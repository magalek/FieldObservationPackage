using System;
using System.Collections.Generic;
using UnityEngine;

namespace FieldObservationPackage.Runtime {
    [CreateAssetMenu(fileName = nameof(ObservedObjectsDataContainer), menuName = "FieldObserver/Data")]
    public class ObservedObjectsDataContainer : ScriptableObject {

        public event Action IDListChanged;

        public List<int> objectIDs = new List<int>();

        public void AddID(int id) {
            if (objectIDs.Contains(id)) {
                return;
            }
            objectIDs.Add(id);
            IDListChanged?.Invoke();
        }

        public void RemoveID(int id) {
            objectIDs.Remove(id);
            IDListChanged?.Invoke();
        }
    }
}
