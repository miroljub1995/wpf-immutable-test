using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WpfImmutableTest.ViewModels
{
    public interface IUpdateFrom<T>
    {
        void UpdateFrom(T source);
    }

    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, Type> generatedTypes = new Dictionary<Type, Type>();
        public static T CreateMutabale<T>(/*this Type type*/)
            where T : class, IUpdateFrom<T>
        {
            //var type = viewModel.GetType();
            var type = typeof(T);
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
            var dictInverse = mappings.ToDictionary((i) => i.Value, (i) => i.Key);
            Type exType;
            if (!mappings.ContainsKey(type) && !dictInverse.ContainsKey(type))
            {
                return type;
            }

            if (dictInverse.ContainsKey(type))
            {
                var tmp = type;
                exType = type;
                type = dictInverse[tmp];
            }
            else
            {
                exType = mappings[type];
            }


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
                var fieldName = "_" + prop.Name.First().ToString().ToLower() + prop.Name.Substring(1);
                Type fieldType;
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(ImmutableList<>))
                {
                    //create observable list field and name it line propType
                    var templateType = CreateExtendedType(propType.GenericTypeArguments[0], mappings);
                    fieldType = typeof(ObservableCollection<>).MakeGenericType(templateType);
                }
                else
                {
                    fieldType = CreateExtendedType(propType, mappings);
                }
                //field
                FieldBuilder fldBuilder = DefineField(tb, fieldName, fieldType);

                //property
                PropertyBuilder propBuilder = DefineProperty(tb, prop.Name, fieldType, fldBuilder);
            }

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static FieldBuilder DefineField(TypeBuilder tb, string fieldName, Type fieldType)
        {
            return tb.DefineField(fieldName, fieldType, FieldAttributes.Private);
        }

        private static PropertyBuilder DefineProperty(TypeBuilder tb, string name, Type type, FieldBuilder fieldToGet)
        {
            PropertyBuilder propBuilder =
                tb.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
            MethodAttributes getAttr =
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            MethodBuilder propGetMthdBldr =
                tb.DefineMethod("get_" + name, getAttr, type, Type.EmptyTypes);
            ILGenerator getMethodIL = propGetMthdBldr.GetILGenerator();

            getMethodIL.Emit(OpCodes.Ldarg_0);
            getMethodIL.Emit(OpCodes.Ldfld, fieldToGet);
            getMethodIL.Emit(OpCodes.Ret);

            propBuilder.SetGetMethod(propGetMthdBldr);
            return propBuilder;
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
