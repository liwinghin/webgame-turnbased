using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MB.Game
{
    [Serializable]
    public class VFXData
    {
        public string vfxTag;
        public GameObject prefab;
        public int size;
    }

    [CreateAssetMenu(fileName = "New VFX Data", menuName = "VFX Data/Create VFX Data Asset", order = 1)]
    public class VFXDataAsset : ScriptableObject
    {
        public List<VFXData> vfxData;

        public List<VFXData> LoadingVFXData()
        {
            return vfxData;
        }
    }
}