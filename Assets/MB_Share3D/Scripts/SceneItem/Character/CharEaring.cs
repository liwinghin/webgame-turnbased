using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharEaring : MonoBehaviour, ICharPart
    {
        public string partName => this.name;

        #region Asset
        public static IEnumerator Load(string path, Action<CharEaring> callBack)
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
            character.nowEaring = this;
            this.SetParent(character.transform);
        }

        public void StopAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Unlisten event
            character.onUpdate.RemoveListener(this.OnUpdate);

            // Remove and Destroy
            character.nowEaring = null;
            Destroy(this.gameObject);
        }
        #endregion

        #region Event handle
        private void OnUpdate(CharRuntime character)
        {
            var body = character.nowBody;
            if (body == null) return;

            this.FollowPosition(body.headAnchor1);
            this.FollowRotation(body.headAnchor1);
        }
        #endregion
    }
}