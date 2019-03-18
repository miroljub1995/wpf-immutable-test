using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfImmutableTest.Store;

namespace WpfImmutableTest.ViewModels
{
    public class ExtendFromImmutableWithTemplAttribute : Attribute
    {
        private Type _type;
        public ExtendFromImmutableWithTemplAttribute(Type type)
        {
            _type = type;
        }

        public Type Type => _type;
    }

    [ExtendFromImmutableWithTempl(typeof(AppState))]
    public partial class AppStateVMTempl
    {

    }

    [ExtendFromImmutableWithTempl(typeof(OtherState))]
    public partial class OtherStateVMTempl
    {
        
    }

    [ExtendFromImmutableWithTempl(typeof(SomeListItem))]
    public partial class SomeListItemVMTempl
    {
        
    }
}
