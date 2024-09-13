using FieldObservationPackage.Runtime;
using UnityEditor;
using UnityEngine;

namespace FieldObservationPackage.Editor {
    
    [ExecuteAlways]
    public class FieldObserverWindow : EditorWindow 
    {
        [SerializeField] private Vector2 scrollPosition;

        private static DataManager dataManager = new DataManager();

        private static bool coldStart;
        
        [MenuItem("Windows/Observed Fields")]
        public static void ShowWindow() {
            GetWindow<FieldObserverWindow>("Observed Fields");
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            dataManager.Initialize();
            coldStart = true;
        }
        
        private void Update() {
            if (dataManager?.observedObjects.Count > 0) {
                Repaint();
            }
        }

        private void OnGUI() {
            if (coldStart)
            {
                foreach (var observedObject in dataManager.observedObjects)
                {
                    observedObject.RebuildReferences();
                }
                coldStart = false;
            }
            Event eventCur = Event.current;
            if (eventCur.type == EventType.DragExited) {
                foreach (var obj in DragAndDrop.objectReferences) {
                    if (obj.GetType() != typeof(GameObject)) {
                        continue;
                    }
                    GameObject castObject = ((GameObject)obj);
                    foreach (var component in castObject.GetComponents<MonoBehaviour>()) {
                        dataManager.AddId(component.GetInstanceID());
                    }
                }
            }

            if (dataManager.observedObjects.Count == 0) {
                EditorGUILayout.LabelField("Drag objects from hierarchy to observe them.");
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginVertical();
            foreach (ObservedObjectData data in dataManager.observedObjects) {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{data.Name} - {data.ObjectType}", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("X", new GUILayoutOption[] { GUILayout.Width(50) } )) {
                    dataManager.RemoveId(data.ObjectID);
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
