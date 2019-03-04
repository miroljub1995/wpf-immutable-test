using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfImmutableTest.Store;

namespace WpfImmutableTest.ViewModels
{
    class AppStateVM : AppState, INotifyPropertyChanged
    {
        public AppStateVM() : base(0, "AppStateVM init", new OtherStateVM())
        {

        }

        public OtherStateVM OtherState => (OtherStateVM)_otherState;

        public void UpdateFrom(AppState state)
        {
            if (_someNumber != state.SomeNumber)
            {
                _someNumber = state.SomeNumber;
                OnPropertyChanged(nameof(SomeNumber));
            }
            if (_someString != state.SomeString)
            {
                _someString = state.SomeString;
                OnPropertyChanged(nameof(SomeString));
            }
            OtherState.UpdateFrom(state.OtherState);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class OtherStateVM : OtherState
    {
        private ObservableCollection<SomeListItem> _someObservableList = new ObservableCollection<SomeListItem>();
        public OtherStateVM() : base(0, "", ImmutableList<SomeListItem>.Empty)
        {

        }

        //public ObservableCollection<SomeListItem> SomeList => _someObservableList;

        public void UpdateFrom(OtherState state)
        {
            _someOtherNumber = state.SomeOtherNumber;
            _someOtherString = state.SomeOtherString;
            _someList = state.SomeList;
        }
    }
}
