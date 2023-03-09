using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class ShipBody : MonoBehaviour, IShipPart
    {
        [Header("Skin")]
        [SerializeField] private Renderer[] m_mainRenders = { };
        [SerializeField] private Renderer[] m_sailRenders = { };
        [SerializeField] private Material m_defaultSkin = null;

        private Material m_bodySkin = null;
        private Material m_sailSkin = null;

        #region Part prop
        public string partName { get => this.name; }
        public string bodySkinName { get => this.m_bodySkin.name; }
        public string sailSkinName { get => this.m_sailSkin.name; }

        public Material bodySkin
        {
            get => this.m_bodySkin;
            set => this.ApplyMaterial(ref this.m_bodySkin, value, this.m_defaultSkin, false, this.m_mainRenders);
        }
        public Material sailSkin
        {
            get => this.m_sailSkin;
            set => this.ApplyMaterial(ref this.m_sailSkin, value, null, true, this.m_sailRenders);
        }
        #endregion

        #region Asset
        public static IEnumerator LoadPart(string path, Action<ShipBody> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }

        public static IEnumerator LoadMaterial(string path, Action<Material> callBack)
        {
            yield return AssetUtils.LoadAsset(path, callBack);
        }
        #endregion

        private void Awake()
        {
            this.InitBodyRender();
            this.InitSailRender();
        }

        #region Part hanle
        public void PreAttach(ShipRuntime ship)
        {
            
        }

        public void StartAttach(ShipRuntime ship)
        {
            // Add and Attach
            ship.nowBody = this;
            this.SetParent(ship.transform);
        }

        public void StopAttach(ShipRuntime ship)
        {
            // Remove and Destroy
            ship.nowBody = null;
            Destroy(this.gameObject);
        }
        #endregion

        #region Skin handle
        private void InitBodyRender()
        {
            this.bodySkin = null;
        }

        private void InitSailRender()
        {
            this.sailSkin = null;
        }
        #endregion
    }
}
    
