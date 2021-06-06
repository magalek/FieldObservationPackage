using System;
using System.Reflection;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Runtime {
    [Serializable]
    public class ObservedFieldData {
        public string FieldName;
        public FieldInfo Info;
        public Object ReflectedObject;
        public object currentValue;

        public ObservedFieldData(string fieldName, FieldInfo info, Object reflectedObject) {
            FieldName = fieldName;
            Info = info;
            ReflectedObject = reflectedObject;
            currentValue = null;
        }

        public void RecalculateValue() {
            currentValue = Info.GetValue(ReflectedObject);
        }
    }
}
