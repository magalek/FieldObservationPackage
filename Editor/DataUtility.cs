using FieldObservationPackage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor {
    public static class DataUtility {

        public static ObservedObjectData GetObjectData(string name, Object observedObject) {
            List<ObservedFieldData> tempList = new List<ObservedFieldData>();
            Type objectType = observedObject.GetType();
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields) {
                ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                if (attribute != null) {
                    tempList.Add(new ObservedFieldData(field.Name, field, observedObject));
                }
            }
            return new ObservedObjectData(name, objectType, tempList, observedObject.GetInstanceID());
        }

    }
}
