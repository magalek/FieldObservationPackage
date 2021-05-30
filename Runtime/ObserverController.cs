using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FieldObservationPackage.Runtime {
    public class ObserverController : MonoBehaviour {

        public List<ObservedObjectData> observedObjectData = new List<ObservedObjectData>();

        public static ObserverController Instance;

        private void Awake() {

            if (Instance == null) {
                Instance = this;
            }
            else {
                if (Instance != this) {
                    Destroy(this);
                }
            }
        }

        private void AddObservedObject(string name, object observedObject) {
            observedObjectData.Add(PopulateList(name, observedObject));
        }

        public static void ObserveObject(string name, object observedObject) {
            if (!Instance) {
                var go = new GameObject("ObserverController");
                go.AddComponent<ObserverController>();
            }
            Instance.AddObservedObject(name, observedObject);
        }

            private ObservedObjectData PopulateList(string name, object observedObject) {
            List<ObservedFieldData> tempList = new List<ObservedFieldData>();
            Type objectType = observedObject.GetType();
            FieldInfo[] fields = objectType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields) {
                ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                if (attribute != null) {
                    tempList.Add(new ObservedFieldData(field, observedObject, attribute.expectedValue, attribute.expectedValuetype));
                }
            }
            return new ObservedObjectData(name, objectType, tempList);
        }


    }

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

    public class ObservedObjectData {
        public readonly string Name;
        public readonly Type ObjectType;
        public readonly List<ObservedFieldData> Fields;

        public ObservedObjectData(string name, Type objectType, List<ObservedFieldData> fields) {
            Name = name;
            ObjectType = objectType;
            Fields = fields;
        }
    }
}
