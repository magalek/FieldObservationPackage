using System;
using System.Collections.Generic;
using UnityEngine;

namespace FieldObservationPackage.Editor
{
    [Serializable]
    public class ObjectsCache
    {
        public static readonly string FilePath = Application.temporaryCachePath + "/field_observer_cache.json";
        
        public List<int> ids = new List<int>();
    }
}