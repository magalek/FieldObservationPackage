using System.Reflection;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Runtime {
    
    public class ObservedFieldData {
        private string FieldName;
        public readonly FieldInfo Info;
        public Object ReflectedObject;
        public object currentValue;

        public ObservedFieldData(string fieldName, FieldInfo info, Object reflectedObject) {
            FieldName = fieldName;
            Info = info;
            ReflectedObject = reflectedObject;
            currentValue = null;
        }

        public void RecalculateValue() => currentValue = Info.GetValue(ReflectedObject);

        public void ReassignObject(Object newObject) => ReflectedObject = newObject;
    }
}
