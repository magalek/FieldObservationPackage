using FieldObservationPackage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor {
    public static class DataUtility
    {
        private static readonly Dictionary<Type, List<(FieldInfo, ObserveFieldAttribute)>> fieldInfoCache = new Dictionary<Type, List<(FieldInfo, ObserveFieldAttribute)>>();

        public static bool TryGetObjectData(string name, Object observedObject, out List<ObservedObjectData> observedObjects)
        {
            observedObjects = new List<ObservedObjectData>();

            var components = ((GameObject)observedObject).GetComponents<Component>();
            //List<Type> types = components.Select(component => component.GetType()).ToList();
           

            bool valid = false;
            
            foreach (var component in components)
            {
                List<ObservedFieldData> tempList = new List<ObservedFieldData>();
                var type = component.GetType();
                int objectID = component.GetInstanceID();
                
                if (!fieldInfoCache.TryGetValue(type, out var fieldsInfo))
                {
                    fieldsInfo = new List<(FieldInfo, ObserveFieldAttribute)>();
                
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
                        ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                        if (attribute == null) continue;
                        fieldsInfo.Add((field, attribute));
                        valid = true;
                    }
                    fieldInfoCache[type] = fieldsInfo;
                }

                foreach (var (field, attribute) in fieldsInfo)
                {
                    tempList.Add(new ObservedFieldData(field.Name, field, component));
                }
            
                if (tempList.Count > 0) observedObjects.Add(new ObservedObjectData(name, type, tempList, objectID));
            }
            

            return valid;
        }

    }
}
