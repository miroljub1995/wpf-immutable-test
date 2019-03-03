using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfImmutableTest.Store;

namespace WpfImmutableTest.ViewModels
{
    class AppStateVM : AppState
    {
        public AppStateVM() : base(0, "", new OtherStateVM())
        {

        }

        public OtherStateVM OtherState => (OtherStateVM)_otherState;

        public void UpdateFrom(AppState state)
        {
            _someNumber = state.SomeNumber;
            _someString = state.SomeString;
            OtherState.UpdateFrom(state.OtherState);
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
