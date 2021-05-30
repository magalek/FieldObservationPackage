using System;
using System.Collections.Generic;

namespace FieldObservationPackage.Runtime {
    public class ObservedObjectData {
        public readonly string Name;
        public readonly Type ObjectType;
        public readonly List<ObservedFieldData> Fields;

        public ObservedObjectData(string name, Type objectType, List<ObservedFieldData> fields) {
            Name = name;
            ObjectType = objectType;
            Fields = fields;
        }
    }
}
