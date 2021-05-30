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

        private Vector2 scrollPosition;

        [MenuItem("Windows/FieldObserver")]
        public static void ShowWindow() {
            GetWindow<FieldObserverWindow>("FieldObserver");
        }

        private void Update() {
            if (!Application.isPlaying) {
                return;
            }
            Repaint();
        }

        private void OnGUI() {
            if (!Application.isPlaying) {
                EditorGUILayout.LabelField("Enter play mode to show data");
                return;
            }
            EditorGUILayout.BeginVertical();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
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
            EditorGUILayout.EndScrollView();
        }
    }
}
