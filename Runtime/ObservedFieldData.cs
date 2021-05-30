using System;
using System.Reflection;

namespace FieldObservationPackage.Runtime {
    public class ObservedFieldData {
        public readonly FieldInfo Info;
        public readonly object ReflectedObject;
        public readonly object ExpectedValue;
        public readonly Type ExpectedValueType;


        public ObservedFieldData(FieldInfo info, object reflectedObject, object expectedValue = null, Type expectedValueType = null) {
            Info = info;
            ReflectedObject = reflectedObject;
            ExpectedValue = expectedValue;
            ExpectedValueType = expectedValueType;
        }
    }
}
