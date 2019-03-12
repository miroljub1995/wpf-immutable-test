using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Redux;
using WpfImmutableTest.Store;
using WpfImmutableTest.ViewModels;

namespace WpfImmutableTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Store<AppState> _store = new Store<AppState>(Reducer.ReduceState, new AppState(0, "Init",
            new OtherState(0, "OtherInit", Enumerable.Range(0, 10000).Select(e => new SomeListItem(e, $"Item num: {e}")).ToImmutableList())));

        private AppStateVM appState = new AppStateVM();

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public MainWindow()
        {
            var ctx = SynchronizationContext.Current;
            InitializeComponent();
            DataContext = appState;
            _store.Subscribe(appState.UpdateFrom);
            RunTasks();


            object ex = ExtendType();
        }

        private void RunTasks()
        {
            var ctx = SynchronizationContext.Current;
            Task.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    _store.Dispatch(new IncrementSomeNumber());
                    Thread.Sleep(1000);
                }
            });
            Task.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    _store.Dispatch(new IncrementSomeNumberInListItem());
                    Thread.Sleep(200);
                }
            });
        }

        private object ExtendType()
        {
            var assembly = new AssemblyName() { Name = "TestAssembly" };
            Type objectType = typeof(AppStateVM);


            AppDomain appDomain = AppDomain.CurrentDomain;
            AssemblyBuilder assemblyBuilder = appDomain.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assembly.Name);

            TypeBuilder typeBuilder = moduleBuilder.DefineType(objectType.Name + "_Mutable", TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                                                            TypeAttributes.BeforeFieldInit, typeof(AppStateVM));

            var fieldBuilder = typeBuilder.DefineField("NewField", typeof(string), FieldAttributes.Public);
            var customType = typeBuilder.CreateType();

            return Activator.CreateInstance(customType);
        }
    }
}
