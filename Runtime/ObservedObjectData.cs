using System;
using System.Collections.Generic;

namespace FieldObservationPackage.Runtime {
    [Serializable]
    public class ObservedObjectData {
        public string Name;
        public Type ObjectType;
        public List<ObservedFieldData> Fields;
        public int ObjectID;

        public ObservedObjectData(string name, Type objectType, List<ObservedFieldData> fields, int objectID) {
            Name = name;
            ObjectType = objectType;
            Fields = fields;
            ObjectID = objectID;          
        }
    }
}
