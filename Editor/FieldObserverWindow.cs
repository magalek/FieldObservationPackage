using FieldObservationPackage.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FieldObservationPackage.Editor {

    [ExecuteAlways]
    [EditorWindowTitle(title = "FieldObserver")]
    public class FieldObserverWindow : EditorWindow {
        [SerializeField] private Vector2 scrollPosition;

        private static ObservedObjectsDataContainer dataContainer;

        private static List<ObservedObjectData> observedObjectData = new List<ObservedObjectData>();
        private List<int> idsToRemove = new List<int>();

        private static bool initialized;

        [MenuItem("Windows/FieldObserver")]
        public static void ShowWindow() {
            GetWindow<FieldObserverWindow>("FieldObserver");
            Initialize();
        }

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

        private static void AssignObjects() {
            observedObjectData.Clear();
            foreach (var id in dataContainer.objectIDs) {
                var obj = EditorUtility.InstanceIDToObject(id);
                AddObservedObject(obj.name, obj);
            }
        }

        private void Update() {
            if (!initialized) {
                Initialize();
            }
            if (observedObjectData.Count > 0) {
                Repaint();
            }
        }

        private static void AddObservedObject(string name, Object observedObject) {
            observedObjectData.Add(DataUtility.GetObjectData(name, observedObject));
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

            if (observedObjectData.Count == 0) {
                EditorGUILayout.LabelField("Drag objects from hierarchy to observe them.");
                return;
            }

            foreach (var id in idsToRemove) {
                dataContainer.RemoveID(id);
            }
            idsToRemove.Clear();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginVertical();
            foreach (ObservedObjectData data in observedObjectData) {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{data.Name} - {data.ObjectType}", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(50) } )) {
                    idsToRemove.Add(data.ObjectID);
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
