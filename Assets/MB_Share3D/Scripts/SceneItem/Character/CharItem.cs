using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CharItem : MonoBehaviour, ICharPart
    {
        private readonly Vector3 m_BackwardOffset = new Vector3(0f ,180f, 0f);

        [SerializeField] private bool m_useBackward = false;

        private CharPartSide m_handSide = CharPartSide.MainPart;

        public string partName => this.name;
        public bool useBackward => this.m_useBackward;
        public CharPartSide handSide { get => this.m_handSide;  private set => this.m_handSide = value; }
        public Vector3 backwardOffset => m_BackwardOffset;

        #region Resource
        public static IEnumerator Load(string path, Action<CharItem> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }
        #endregion

        #region Part hanle
        private Transform GetAnchorPoint(CharRuntime character)
        {
            if (character.nowBody == null)
                return null;
            var body = character.nowBody;

            if (this.handSide == CharPartSide.LeftHand)
            {
                return body.handAnchorLeft;
            }
            if (this.handSide == CharPartSide.RightHand)
            {
                return body.handAnchorRight;
            }
            return null;
        }

        public void PreAttach(CharRuntime character, CharPartSide side = CharPartSide.MainPart)
        {
            // Set data
            this.handSide = side;

            // Listen event
            character.onUpdate.AddListener(this.OnUpdate);
        }

        public void StartAttach(CharRuntime character, CharPartSide side)
        {
            // Set data
            this.handSide = side;

            // Listen event
            character.onUpdate.AddListener(this.OnUpdate);

            // Add and Attach
            if (this.handSide == CharPartSide.LeftHand)
            {
                character.nowLeftHandItems.Add(this);
                this.SetParent(character.transform);
            }
            if (this.handSide == CharPartSide.RightHand)
            {
                character.nowRightHandItems.Add(this);
                this.SetParent(character.transform);
            }
        }

        public void StopAttach(CharRuntime character, CharPartSide side)
        {
            // Unlisten event
            character.onUpdate.RemoveListener(this.OnUpdate);

            // Remove and Destroy
            if (this.handSide == CharPartSide.LeftHand)
            {
                character.nowLeftHandItems.Remove(this);
                Destroy(this.gameObject);
            }
            if (this.handSide == CharPartSide.RightHand)
            {
                character.nowRightHandItems.Remove(this);
                Destroy(this.gameObject);
            }
        }
        #endregion

        #region Event handle
        private void OnUpdate(CharRuntime character)
        {
            var anchorPoint = this.GetAnchorPoint(character);
            if (anchorPoint == null) return;


            this.FollowPosition(anchorPoint);
            if (this.useBackward)
            {
                this.FollowRotation(anchorPoint, this.backwardOffset);
            }
            else
            {
                this.FollowRotation(anchorPoint);
            }
        }
        #endregion
    }
}