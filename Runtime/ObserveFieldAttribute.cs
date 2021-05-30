using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FieldObservationPackage.Runtime {
    [AttributeUsage(AttributeTargets.Field)]
    public class ObserveFieldAttribute : Attribute {

        public readonly object expectedValue;
        public readonly Type expectedValuetype;

        public ObserveFieldAttribute() {

        }

        public ObserveFieldAttribute(Type _type ,object _expectedValue) {
            expectedValue = _expectedValue;
            expectedValuetype = _type;
        }
    }
}
