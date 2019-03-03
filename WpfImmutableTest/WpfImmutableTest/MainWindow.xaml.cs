using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
            new OtherState(0, "OtherInit",
                ImmutableList<SomeListItem>.Empty.AddRange(new[]
                    {new SomeListItem(3, "Item string 1"), new SomeListItem(4, "Item string 2")}))));

        private AppStateVM appState = new AppStateVM();

        public MainWindow()
        {
            var ctx = SynchronizationContext.Current;
            InitializeComponent();
            DataContext = appState;
            _store.Subscribe(state => ctx.Post(_ => appState.UpdateFrom(state), null));
            RunTasks();
        }

        private void RunTasks()
        {
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
                    Thread.Sleep(100);
                }
            });
        }
    }
}
