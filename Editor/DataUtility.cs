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

        public static bool TryGetObjectData(string name, Object observedObject, out ObservedObjectData observedObjectData) {
            List<ObservedFieldData> tempList = new List<ObservedFieldData>();
            Type objectType = observedObject.GetType();
            int objectID = observedObject.GetInstanceID();
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            bool foundAttribute = false;
            foreach (var field in fields) {
                ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                if (attribute != null) {
                    tempList.Add(new ObservedFieldData(field.Name, field, observedObject));
                    foundAttribute = true;
                }
            }
            observedObjectData = new ObservedObjectData(name, objectType, tempList, objectID);

            return foundAttribute;
        }

    }
}
