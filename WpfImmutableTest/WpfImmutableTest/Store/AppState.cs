using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfImmutableTest.Store
{
    public class AppState
    {
        protected int _someNumber;
        protected string _someString;
        protected OtherState _otherState;

        public AppState(int someNumber, string someString, OtherState otherState)
        {
            _someNumber = someNumber;
            _someString = someString;
            _otherState = otherState;
        }

        public int SomeNumber => _someNumber;
        public string SomeString => _someString;
        public OtherState OtherState => _otherState;
    }

    public class OtherState
    {
        protected int _someOtherNumber;
        protected string _someOtherString;
        protected ImmutableList<SomeListItem> _someList;

        public OtherState(int someOtherNumber, string someOtherString, ImmutableList<SomeListItem> someList)
        {
            _someOtherNumber = someOtherNumber;
            _someOtherString = someOtherString;
            _someList = someList;
        }

        public virtual int SomeOtherNumber => _someOtherNumber;
        public virtual string SomeOtherString => _someOtherString;
        public ImmutableList<SomeListItem> SomeList => _someList;
    }

    public class SomeListItem
    {
        protected int _itemNumber;
        protected string _itemString;

        public SomeListItem(int itemNumber, string itemString)
        {
            _itemNumber = itemNumber;
            _itemString = itemString;
        }

        public virtual int ItemNumber => _itemNumber;
        public virtual string ItemString => _itemString;
    }
}
