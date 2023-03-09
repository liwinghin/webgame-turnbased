using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharHair : MonoBehaviour, ICharPart
    {
        [SerializeField] private bool m_useUpward = false;

        public string partName => this.name;
        public bool useUpward => this.m_useUpward;

        #region Asset
        public static IEnumerator Load(string path, Action<CharHair> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }
        #endregion

        #region Part hanle
        public void PreAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Listen event
            character.onUpdate.AddListener(this.OnUpdate);
        }

        public Transform GetAnchorPoint(CharRuntime character)
        {
            if (character.nowBody == null)
                return null;
            var body = character.nowBody;

            if (this.useUpward)
                return body.headAnchor2;

            return body.headAnchor1;
        }

        public void StartAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Listen event
            character.onUpdate.AddListener(this.OnUpdate);

            // Add and Attach
            character.nowHair = this;
            this.SetParent(character.transform);
        }

        public void StopAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Unlisten event
            character.onUpdate.RemoveListener(this.OnUpdate);

            // Remove and Destroy
            character.nowHair = null;
            Destroy(this.gameObject);
        }
        #endregion

        #region Event handle
        private void OnUpdate(CharRuntime character)
        {
            var anchorPoint = this.GetAnchorPoint(character);
            if (anchorPoint == null) return;

            this.FollowPosition(anchorPoint);
            this.FollowRotation(anchorPoint);
        }
        #endregion
    }
}