using System;
using System.Collections.Generic;
using UnityEditor;

namespace FieldObservationPackage.Runtime {

    public class ObservedObjectData {
        public readonly string Name;
        public readonly Type ObjectType;
        public readonly List<ObservedFieldData> Fields;
        public readonly int ObjectID;

        public ObservedObjectData(string name, Type objectType, List<ObservedFieldData> fields, int objectID) {
            Name = name;
            ObjectType = objectType;
            Fields = fields;
            ObjectID = objectID;          
        }

        public void RebuildReferences() =>
            Fields.ForEach(f => f.ReassignObject(EditorUtility.InstanceIDToObject(ObjectID)));
    }
}
