using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WpfImmutableTest.ViewModels
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, Type> generatedTypes = new Dictionary<Type, Type>();
        public static T Extend<T>(this T viewModel)
            where T : class, new()
        {
            var type = viewModel.GetType();
            if (!generatedTypes.ContainsKey(type))
            {
                var mappings = GetTypesMappings();
                var newType = CreateExtendedType(type, mappings);

                generatedTypes[type] = newType;
            }
            return (T)Activator.CreateInstance(generatedTypes[type]);
        }

        public static Type CreateExtendedType(Type type, Dictionary<Type, Type> mappings)
        {
            if (!mappings.ContainsKey(type))
            {
                return type;
            }

            var exType = mappings[type];

            var typeName = type.Name + "_Extended";
            var an = new AssemblyName(typeName);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("ExtensionModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeName,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                type);
            //ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            var props = exType.GetProperties().Where(p => p.CanRead).ToList();
            foreach(var prop in props)
            {
                var propType = prop.PropertyType;
                if(propType.IsGenericType && prop == typeof(ImmutableList<>))
                {
                    //create observable list field and name it line propType
                }
                else
                {
                    var newType = CreateExtendedType(propType, mappings);
                }
            }

            Type objectType = tb.CreateType();
            return objectType;
        }

        static Dictionary<Type, Type> GetTypesMappings()
        {
            var mappings = new Dictionary<Type, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attrs = type.GetCustomAttributes(typeof(ExtendFromImmutableAttribute), true);
                    if (attrs.Length == 1)
                    {
                        var attr = (ExtendFromImmutableAttribute) attrs[0];
                        mappings[type] = attr.Type;
                    }
                }
            }
            return mappings;
        }
    }
    public class ExtendFromImmutableAttribute : Attribute
    {
        private Type _type;
        public ExtendFromImmutableAttribute(Type type)
        {
            _type = type;
        }

        public Type Type => _type;
    }
}
