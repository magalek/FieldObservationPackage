using FieldObservationPackage.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor {

    [ExecuteAlways]
    public class FieldObserverWindow : EditorWindow {
        [SerializeField] private Vector2 scrollPosition;

        private static ObservedObjectsDataContainer dataContainer;

        private static List<ObservedObjectData> observedObjectsData = new List<ObservedObjectData>();
        private static Queue<int> idsToRemoveQueue = new Queue<int>();

        private static bool initialized;
        private static bool assigningObjects;

        [MenuItem("Windows/FieldObserver")]
        public static void ShowWindow() {
            GetWindow<FieldObserverWindow>("FieldObserver");
            Initialize();
        }

        [InitializeOnLoadMethod]
        private static void Initialize() {
            if (initialized) {
                return;
            }
            dataContainer = AssetDatabase.LoadAssetAtPath<ObservedObjectsDataContainer>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ObservedObjectsDataContainer")[0]));
            EditorApplication.playModeStateChanged += AssignObjects;
            dataContainer.IDListChanged += AssignObjects;
            
            initialized = true;
        }
        
        private static void AssignObjects(PlayModeStateChange stateChange) {
            if (stateChange != PlayModeStateChange.EnteredPlayMode) {
                return;
            }
            AssignObjects();
        }

        [DidReloadScripts]
        private static void AssignObjects() {
            observedObjectsData.Clear();
            foreach (var id in dataContainer.objectIDs) {
                var obj = EditorUtility.InstanceIDToObject(id);
                AddObservedObject(obj.name, obj);
            }
        }

        private void Update() {
            if (!initialized) {
                Initialize();
            }

            if (idsToRemoveQueue.Count > 0) {
                while (idsToRemoveQueue.Count > 0) {
                    dataContainer.RemoveID(idsToRemoveQueue.Dequeue());
                }
            }

            if (observedObjectsData.Count > 0) {
                Repaint();
            }
        }

        private static void AddObservedObject(string name, Object observedObject) {

            if (DataUtility.TryGetObjectData(name, observedObject, out ObservedObjectData observedObjectData)) {
                observedObjectsData.Add(observedObjectData);
            }
            else {
                idsToRemoveQueue.Enqueue(observedObjectData.ObjectID);
            }
            AssetDatabase.SaveAssets();
        }

        private void OnGUI() {
            Event eventCur = Event.current;
            if (eventCur.type == EventType.DragExited) {
                foreach (var obj in DragAndDrop.objectReferences) {
                    if (obj.GetType() != typeof(GameObject)) {
                        continue;
                    }
                    GameObject castedObject = ((GameObject)obj);
                    foreach (var component in castedObject.GetComponents<MonoBehaviour>()) {
                        dataContainer.AddID(component.GetInstanceID());
                    }
                }
            }

            if (observedObjectsData.Count == 0) {
                EditorGUILayout.LabelField("Drag objects from hierarchy to observe them.");
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginVertical();
            foreach (ObservedObjectData data in observedObjectsData) {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{data.Name} - {data.ObjectType}", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(50) } )) {
                    idsToRemoveQueue.Enqueue(data.ObjectID);
                }
                EditorGUILayout.EndHorizontal();
                foreach (var fieldData in data.Fields) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField(fieldData.Info.Name);
                    //object value = fieldData.Info.GetValue(fieldData.ReflectedObject);
                    if (Application.isPlaying) {
                        var defaultColor = GUI.color;
                        fieldData.RecalculateValue();
                        EditorGUILayout.LabelField(fieldData.currentValue.ToString() ?? "0");
                        GUI.color = defaultColor;
                    }
                    else {
                        EditorGUILayout.LabelField("-");
                    }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}
