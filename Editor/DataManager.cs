using System.Collections.Generic;
using System.IO;
using FieldObservationPackage.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor
{
    public class DataManager
    {
        private ObjectsCache cache;

        private void SaveCache()
        {
            File.WriteAllText(ObjectsCache.FilePath, JsonUtility.ToJson(cache));
        }
        
        private ObjectsCache LoadCache()
        {
            if (!File.Exists(ObjectsCache.FilePath))
            {
                var tempCache = new ObjectsCache();
                SaveCache();
                return tempCache;
            }
            var output =JsonUtility.FromJson<ObjectsCache>(File.ReadAllText(ObjectsCache.FilePath));
            return output ?? new ObjectsCache();
        }

        private readonly Queue<int> idsToAdd = new Queue<int>();
        private readonly Queue<int> idsToRemove = new Queue<int>();
        
        public readonly List<ObservedObjectData> observedObjects = new List<ObservedObjectData>();
        
        public void Initialize()
        {
            cache = LoadCache();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.quitting += SaveCache;
            AssignObjects();
        }

        private void OnPlayModeStateChanged(PlayModeStateChange stateChange) {
            if (stateChange is PlayModeStateChange.ExitingPlayMode or PlayModeStateChange.ExitingEditMode) SaveCache();
        }
        
        private void AssignObjects()
        {
            if (cache?.ids == null) return;
            foreach (var id in cache.ids) {
                var obj = EditorUtility.InstanceIDToObject(id);
                if (obj != null) AddObject(obj.name, obj);
                else idsToRemove.Enqueue(id);
            }

            foreach (var id in idsToRemove)
            {
                RemoveId(id);
            }
        }
        
        
        private void AddObject(string name, Object observedObject) {

            if (DataUtility.TryGetObjectData(name, observedObject, out List<ObservedObjectData> observedObjectsDatas)) {
                observedObjects.AddRange(observedObjectsDatas);
            }
            else {
                foreach (var obj in observedObjects)
                {
                    idsToRemove.Enqueue(obj.ObjectID);
                }
            }
        }

        public void AddId(int id)
        {
            cache.ids.Add(id);
            var obj = EditorUtility.InstanceIDToObject(id);
            AddObject(obj.name, obj);
        }
        
        public void RemoveId(int id)
        {
            cache.ids.Remove(id);
        }

        ~DataManager()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.quitting -= SaveCache;
        }
    }
}