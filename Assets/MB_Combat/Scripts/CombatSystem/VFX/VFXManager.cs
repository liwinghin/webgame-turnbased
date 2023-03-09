using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class VFXManager : MonoBehaviour
    {
        private static VFXManager m_Instance = null;
        public static VFXManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<VFXManager>();
                return m_Instance;
            }
        }

        public VFXDataAsset vfxDataAsset = null;
        public List<VFXData> vfxPools = new List<VFXData>();
        public Dictionary<string, Queue<GameObject>> vfxDictionary;
        
        void Awake()
        {
            vfxPools = vfxDataAsset.LoadingVFXData();
            vfxDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (VFXData vfx in vfxPools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for(int i = 0; i < vfx.size; i++)
                {
                    GameObject obj = Instantiate(vfx.prefab, transform);
                    VFXHandler vfxHandler = obj.GetComponent<VFXHandler>();
                    vfxHandler.initParticle();
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                vfxDictionary.Add(vfx.vfxTag, objectPool);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
        {
            if (!vfxDictionary.ContainsKey(tag)) { return null; }

            GameObject obj = vfxDictionary[tag].Dequeue();
            obj.SetActive(true);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            return obj;
        }

        public void Recovery(string tag, GameObject recovery)
        {
            vfxDictionary[tag].Enqueue(recovery);
            recovery.SetActive(false);
        }
    }
}
