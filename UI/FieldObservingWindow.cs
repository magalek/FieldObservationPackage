using System;
using System.Collections.Generic;
using System.Linq;
using FieldObservationPackage.Editor;
using FieldObservationPackage.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class FieldObservingWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    
    private static DataManager dataManager = new DataManager();
    
    [MenuItem("Window/UI Toolkit/FieldObservingWindow")]
    public static void ShowExample()
    {
        FieldObservingWindow wnd = GetWindow<FieldObservingWindow>();
        wnd.titleContent = new GUIContent("Field Observing Window");
    }
    
    public void CreateGUI()
    {
        dataManager.Initialize();
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();

        root.Add(labelFromUXML);

        var listView = root.Q<ListView>("ListView");
        listView.itemsSource = dataManager.observedObjects;
        listView.bindItem = BindListItem;

        listView.handleDrop += ListViewOnhandleDrop;
        listView.dragAndDropUpdate += ListViewOndragAndDropUpdate;
    }

    private DragVisualMode ListViewOndragAndDropUpdate(HandleDragAndDropArgs arg) => DragVisualMode.Copy;

    private DragVisualMode ListViewOnhandleDrop(HandleDragAndDropArgs arg)
    {
        foreach (var reference in arg.dragAndDropData.unityObjectReferences)
        {
            dataManager.AddId(reference.GetInstanceID());
        }
        rootVisualElement.Q<ListView>("ListView").RefreshItems();
        return DragVisualMode.None;
    }
    
    private void BindListItem(VisualElement element, int index)
    {
        ObservedObjectData data = dataManager.observedObjects[index];
        
        var foldout = element.Q<Foldout>("object_foldout");
        foldout.text = $"{data.Name} - <b>{data.ObjectType}</b>";
        var propertyList = foldout.Q<ListView>();
        propertyList.itemsSource = data.Fields;
        propertyList.bindItem += (e, i) => e.Q<ObservedObjectBox>().SetField(data.Fields[i]);
    }
}


