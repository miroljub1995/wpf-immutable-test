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

        public new OtherStateVM OtherState => (OtherStateVM)_otherState;

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

    class OtherStateVM : OtherState, INotifyPropertyChanged
    {
        private ObservableCollection<SomeListItemVM> _someObservableList = new ObservableCollection<SomeListItemVM>();
        public OtherStateVM() : base(0, "", ImmutableList<SomeListItem>.Empty)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new ObservableCollection<SomeListItemVM> SomeList => _someObservableList;

        public void UpdateFrom(OtherState state)
        {
            if (_someOtherNumber != state.SomeOtherNumber)
            {
                _someOtherNumber = state.SomeOtherNumber;
                OnPropertyChanged(nameof(SomeOtherNumber));
            }
            if (_someOtherString != state.SomeOtherString)
            {
                _someOtherString = state.SomeOtherString;
                OnPropertyChanged(nameof(SomeOtherString));
            }
            if (_someList != state.SomeList)
            {
                _someList = state.SomeList;
                UpdateObservableCollection(_someList);
            }
        }

        private void UpdateObservableCollection(ImmutableList<SomeListItem> list)
        {
            while (list.Count < _someObservableList.Count)
            {
                _someObservableList.RemoveAt(_someObservableList.Count - 1);
            }
            while (list.Count > _someObservableList.Count)
            {
                _someObservableList.Add(new SomeListItemVM());
            }
            //for (int i = 0; i < _someObservableList.Count; i++)
            //{
            //    _someObservableList[i].UpdateFrom(list[i]);
            //}
            Parallel.For(0, _someObservableList.Count, (i) => _someObservableList[i].UpdateFrom(list[i]));
        }
    }

    class SomeListItemVM : SomeListItem, INotifyPropertyChanged
    {
        public SomeListItemVM() : base(0, "")
        {

        }

        public void UpdateFrom(SomeListItem item)
        {
            if (_itemNumber != item.ItemNumber)
            {
                _itemNumber = item.ItemNumber;
                OnPropertyChanged(nameof(ItemNumber));
            }
            if (_itemString != item.ItemString)
            {
                _itemString = item.ItemString;
                OnPropertyChanged(nameof(ItemString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
