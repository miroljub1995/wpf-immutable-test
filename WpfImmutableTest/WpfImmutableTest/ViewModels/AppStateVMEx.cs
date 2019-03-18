using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfImmutableTest.Store;

namespace WpfImmutableTest.ViewModels
{
    [ExtendFromImmutable(typeof(AppState))]
    public abstract class AppStateVMEx : IUpdateFrom<AppState>
    {
        public abstract void UpdateFrom(AppState source);
    }

    [ExtendFromImmutable(typeof(OtherState))]
    public abstract class OtherStateVMEx : IUpdateFrom<OtherState>
    {
        public abstract void UpdateFrom(OtherState source);
    }

    [ExtendFromImmutable(typeof(SomeListItem))]
    public abstract class SomeListItemVMEx : IUpdateFrom<SomeListItem>
    {
        public abstract void UpdateFrom(SomeListItem source);
    }
}
