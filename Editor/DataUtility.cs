using FieldObservationPackage.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor {
    public static class DataUtility
    {

        private static readonly Dictionary<Type, List<(FieldInfo, ObserveFieldAttribute)>> fieldInfoCache = new Dictionary<Type, List<(FieldInfo, ObserveFieldAttribute)>>();

        public static bool TryGetObjectData(string name, Object observedObject, out ObservedObjectData observedObjectData) {
            List<ObservedFieldData> tempList = new List<ObservedFieldData>();
            Type objectType = observedObject.GetType();
            int objectID = observedObject.GetInstanceID();
            if (!fieldInfoCache.TryGetValue(objectType, out var fieldsInfo))
            {
                fieldsInfo = new List<(FieldInfo, ObserveFieldAttribute)>();
                
                foreach (var field in objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                    ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                    if (attribute == null) continue;
                    fieldsInfo.Add((field, attribute));
                }
                fieldInfoCache[objectType] = fieldsInfo;
            }

            foreach (var (field, attribute) in fieldsInfo)
            {
                tempList.Add(new ObservedFieldData(field.Name, field, observedObject));
            }
            
            observedObjectData = new ObservedObjectData(name, objectType, tempList, objectID);

            return fieldsInfo.Count > 0;
        }

    }
}
