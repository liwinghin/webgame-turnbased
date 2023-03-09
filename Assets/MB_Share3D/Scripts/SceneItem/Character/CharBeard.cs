using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharBeard : MonoBehaviour, ICharPart
    {
        public string partName => this.name;

        #region Asset
        public static IEnumerator Load(string path, Action<CharBeard> callBack)
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

        public void StartAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Listen event
            character.onUpdate.AddListener(this.OnUpdate);

            // Add and Attach
            character.nowBeard = this;
            this.SetParent(character.transform);
        }

        public void StopAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Unlisten event
            character.onUpdate.RemoveListener(this.OnUpdate);

            // Remove and Destroy
            character.nowBeard = null;
            Destroy(this.gameObject);
        }
        #endregion

        #region Event handle
        private void OnUpdate(CharRuntime character)
        {
            var body = character.nowBody;
            if (body == null) return;

            this.FollowPosition(body.headAnchor2);
            this.FollowRotation(body.headAnchor2);
        }
        #endregion
    }
}