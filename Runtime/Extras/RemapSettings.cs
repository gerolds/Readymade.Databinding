using System;
using UnityEngine;

namespace Readymade.Databinding {
    [Serializable]
    public struct RemapSettings {
        public float FromMin;
        public float FromMax;
        public float ToMin;
        public float ToMax;
    }
    
    [Serializable]
    public struct RemapSettingsInt {
        public int FromMin;
        public int FromMax;
        public int ToMin;
        public int ToMax;
    }
    
    [Serializable]
    public struct RemapSettingsLong {
        public long FromMin;
        public long FromMax;
        public long ToMin;
        public long ToMax;
    }
}