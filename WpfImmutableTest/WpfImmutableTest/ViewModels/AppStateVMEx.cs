using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfImmutableTest.Store;

namespace WpfImmutableTest.ViewModels
{
    [ExtendFromImmutable(typeof(AppState))]
    public class AppStateVMEx
    {

    }

    [ExtendFromImmutable(typeof(OtherState))]
    public class OtherStateVMEx
    {
        
    }

    [ExtendFromImmutable(typeof(SomeListItem))]
    public class SomeListItemVMEx
    {
        
    }
}
