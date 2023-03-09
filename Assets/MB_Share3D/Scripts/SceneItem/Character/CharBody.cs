using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharBody : MonoBehaviour, ICharPart
    {
        [Header("Skin")]
        [SerializeField] private Renderer[] m_mainRenders = { };
        [SerializeField] private Material m_defaultSkin = null;

        [Header("Anchor")]
        [SerializeField] private Transform m_centerAnchor = null;
        [SerializeField] private Transform m_headAnchor1 = null;
        [SerializeField] private Transform m_headAnchor2 = null;
        [SerializeField] private Transform m_handAnchorLeft = null;
        [SerializeField] private Transform m_handAnchorRight = null;

        [Header("Animation")]
        [SerializeField] private Animator m_animator = null;

        private Renderer[] m_energyRenderers = { };

        private Material m_bodySkin = null;
        private Material m_energySkin = null;

        public string partName => this.name;
        public string bodySkinName => this.GetBodySkinName();
        public string energySkinName => this.GetEnergySkinName();

        public Material bodySkin
        {
            get => this.m_bodySkin;
            set => this.ApplyMaterial(ref this.m_bodySkin, value, this.m_defaultSkin, false, this.m_mainRenders);
        }
        public Material energySkin
        {
            get => this.m_energySkin;
            set => this.ApplyMaterial(ref this.m_energySkin, value, null, true, this.m_energyRenderers);
        }

        public Transform centerAnchor => this.m_centerAnchor;
        public Transform headAnchor1 => this.m_headAnchor1;
        public Transform headAnchor2 => this.m_headAnchor2;
        public Transform handAnchorLeft => this.m_handAnchorLeft;
        public Transform handAnchorRight => this.m_handAnchorRight;

        private void Awake()
        {
            this.InitBodyRender();
            this.InitEnemyRender();
        }

        #region Asset
        public static IEnumerator LoadPart(string path, Action<CharBody> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }

        public static IEnumerator LoadBodySkin(string path, Action<Material> callBack)
        {
            yield return AssetUtils.LoadAsset(path, callBack);
        }

        public static IEnumerator LoadEnergySkin(string path, Action<Material> callBack)
        {
            yield return AssetUtils.LoadAsset(path, callBack);
        }
        #endregion

        #region Part hanle
        public void PreAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Listen event
            character.onLoopMotion.AddListener(this.OnLoopMotion);
            character.onOnceMotion.AddListener(this.OnOnceMotion);
        }

        public void StartAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Listen event
            character.onLoopMotion.AddListener(this.OnLoopMotion);
            character.onOnceMotion.AddListener(this.OnOnceMotion);

            // Add and Attach
            character.nowBody = this;
            this.SetParent(character.transform);
        }

        public void StopAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Unlisten event
            character.onLoopMotion.RemoveListener(this.OnLoopMotion);
            character.onOnceMotion.RemoveListener(this.OnOnceMotion);

            // Remove and Destroy
            character.nowBody = null;
            Destroy(this.gameObject);
        }

        private string GetBodySkinName()
        {
            if (this.m_bodySkin == null)
                return string.Empty;
            return this.m_bodySkin.name;
        }

        private string GetEnergySkinName()
        {
            if (this.m_energySkin == null)
                return string.Empty;
            return this.m_energySkin.name;
        }
        #endregion

        #region Skin handle
        private void InitBodyRender()
        {
            this.bodySkin = null;
        }

        private void InitEnemyRender()
        {
            var total = this.m_mainRenders.Length;
            this.m_energyRenderers = new Renderer[total];
            for (var i = 0; i < total; i++)
            {
                var mainRender = this.m_mainRenders[i];

                var gObj = new GameObject(mainRender.name);
                gObj.transform.parent = transform;

                if (mainRender is SkinnedMeshRenderer)
                {
                    var mainSRender = (SkinnedMeshRenderer)mainRender;
                    var newSRender = gObj.AddComponent<SkinnedMeshRenderer>();
                    newSRender.bones = mainSRender.bones;
                    newSRender.rootBone = mainSRender.rootBone;
                    newSRender.sharedMesh = mainSRender.sharedMesh;
                    this.m_energyRenderers[i] = newSRender;
                }
                else
                {
                    var newRender = gObj.AddComponent<MeshRenderer>();
                    this.m_energyRenderers[i] = newRender;
                }
            }

            this.energySkin = null;
        }
        #endregion

        #region Animation handle
        private void ForceIdle()
        {
            this.m_animator.Play("Idle", -1, 0.0f);
        }
        #endregion

        #region Event handle
        private void OnLoopMotion(CharLoopMotion motionValue)
        {
            var index = (int)motionValue;
            var keys = CharConfig.GetLoopMotionKeys();
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                if (!string.IsNullOrEmpty(key))
                {
                    this.m_animator.SetBool(key, i == index);
                }
            }
            if (index == 0)
            {
                this.ForceIdle();
            }
        }

        private void OnOnceMotion(CharOnceMotion motionValue)
        {
            var key = CharConfig.GetOnceMotionKey(motionValue);
            if (!string.IsNullOrEmpty(key))
            {
                this.m_animator.SetTrigger(key);
            }
        }
        #endregion
    }
}