using FieldObservationPackage.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace FieldObservationPackage.Editor {

    [EditorWindowTitle(title = "FieldObserver")]
    public class FieldObserverWindow : EditorWindow {

        private List<FieldInfo> observedFields = new List<FieldInfo>();

        [MenuItem("Windows/FieldObserverWindow")]
        public static void ShowWindow() {
            GetWindow<FieldObserverWindow>("FieldObserver");
            FindObservedFields();
        }

        private static void FindObservedFields() {
            Assembly asm = Assembly.GetCallingAssembly();
            Type[] types = asm.GetTypes();

            foreach (var type in types) {
                FieldInfo[] fields = type.GetFields();
                foreach (var field in fields) {
                    ObserveFieldAttribute attribute = field.GetCustomAttribute<ObserveFieldAttribute>();
                    if (attribute != null) {
                        Debug.Log(field.Name);
                    }
                }
                
            }
        }

        private void Update() {
            Repaint();
        }

        private void OnGUI() {
            if (!Application.isPlaying) {
                return;
            }
            Profiler.BeginSample("WINDOW GUI");
            EditorGUILayout.BeginVertical();
            foreach (ObservedObjectData data in ObserverController.Instance.observedObjectData) {
                EditorGUILayout.LabelField($"{data.Name} - {data.ObjectType}");
                foreach (var fieldData in data.Fields) {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField(fieldData.Info.Name);
                    object value = fieldData.Info.GetValue(fieldData.ReflectedObject);
                    var defaultColor = GUI.color;
                    if (fieldData.ExpectedValue != null) {
                        bool equal;
                        try {
                            equal = Convert.ChangeType(value, fieldData.ExpectedValueType).Equals(Convert.ChangeType(fieldData.ExpectedValue, fieldData.ExpectedValueType));
                        }
                        catch (Exception) {

                            throw;
                        }
                        GUI.color = equal ? Color.green : Color.red;
                    }
                    
                    EditorGUILayout.LabelField(value.ToString());
                    GUI.color = defaultColor;
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();
                }
                
            }
            EditorGUILayout.EndVertical();
            Profiler.EndSample();
        }
    }
}
