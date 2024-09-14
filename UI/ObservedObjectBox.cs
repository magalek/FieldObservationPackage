using FieldObservationPackage.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
partial class ObservedObjectBox : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ObservedObjectBox> {}

    private Label nameLabel;
    private Label valueLabel;
    
    public ObservedObjectBox()
    {
        //hierarchy.parent.
        nameLabel = new Label("Field name");
        valueLabel = new Label("Field value");
        nameLabel.name = "name";
        valueLabel.name = "value";
        Add(nameLabel);
        Add(valueLabel);
        
    }

    public void SetField(ObservedFieldData data)
    {
        nameLabel.text = $"{data.Info.Name} ({data.Info.FieldType.Name})";
        valueLabel.text = data.currentValue != null ? data.currentValue.ToString() : "null";
        valueLabel.schedule.Execute(() => UpdateValue(valueLabel, data)).Every(16);
    }
    
    private void UpdateValue(Label label, ObservedFieldData data)
    {
        data.RecalculateValue();
        label.text = data.currentValue != null ? data.currentValue.ToString() : "null";
    }
}