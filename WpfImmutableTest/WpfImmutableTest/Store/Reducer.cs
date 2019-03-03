using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux;

namespace WpfImmutableTest.Store
{
    class Reducer
    {
        public static AppState ReduceState(AppState state, IAction action)
        {
            if (action is IncrementSomeNumber)
            {
                return new AppState(state.SomeNumber + 1, state.SomeString, state.OtherState);
            }
            if (action is IncrementSomeNumberInListItem)
            {
                var list = state.OtherState.SomeList;
                var newItem = new SomeListItem(list[0].ItemNumber + 1, list[0].ItemString);
                list = list.Replace(list[0], newItem);
                var newEl = new OtherState(state.OtherState.SomeOtherNumber, state.OtherState.SomeOtherString, list);
                return new AppState(state.SomeNumber, state.SomeString, newEl);
            }
            return state;
        }
    }
}
