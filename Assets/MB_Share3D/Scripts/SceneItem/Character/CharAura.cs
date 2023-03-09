using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharAura : MonoBehaviour, ICharPart
    {
        public string partName => this.name;

        #region Asset
        public static IEnumerator Load(string path, Action<CharAura> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }
        #endregion

        #region Part handle
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
            character.nowAura = this;
            this.SetParent(character.transform);
        }

        public void StopAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Unlisten event
            character.onUpdate.RemoveListener(this.OnUpdate);

            // Remove and Destroy
            character.nowAura = null;
            Destroy(this.gameObject);
        }
        #endregion

        #region Event handle
        private void OnUpdate(CharRuntime character)
        {
            var body = character.nowBody;
            if (body == null) return;

            this.FollowPosition(body.centerAnchor);
            this.FollowRotation(body.centerAnchor);
        }
        #endregion
    }
}