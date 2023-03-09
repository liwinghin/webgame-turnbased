using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace MB
{
    public class CharRuntime : MonoBehaviour
    {
        [Header("Main Parts")]
        [SerializeField] private CharBody m_nowBody = null;
        [SerializeField] private CharFace m_nowFace = null;
        [SerializeField] private CharHair m_nowHair = null;
        [SerializeField] private CharEyewear m_nowEyewear = null;
        [SerializeField] private CharEaring m_nowEaring = null;
        [SerializeField] private CharBeard m_nowBeard = null;
        [SerializeField] private CharHat m_nowHat = null;
        [SerializeField] private CharAura m_nowAura = null;

        [Header("Hand Parts")]
        [SerializeField] private List<CharItem> m_nowLeftHandItems = new List<CharItem>();
        [SerializeField] private List<CharItem> m_nowRightHandItems = new List<CharItem>();

        [Header("Event")]
        public UnityEvent<CharRuntime> onUpdate = new UnityEvent<CharRuntime>();
        public UnityEvent<CharLoopMotion> onLoopMotion = new UnityEvent<CharLoopMotion>();
        public UnityEvent<CharOnceMotion> onOnceMotion = new UnityEvent<CharOnceMotion>();

        private CharLoopMotion m_loopMotion = CharLoopMotion.Idle;

        #region Part prop
        public CharBody nowBody
        {
            get => this.m_nowBody; set => this.m_nowBody = value;
        }
        public CharFace nowFace
        {
            get => this.m_nowFace; set => this.m_nowFace = value;
        }
        public CharHair nowHair
        {
            get => this.m_nowHair; set => this.m_nowHair = value;
        }
        public CharEyewear nowEyewear
        {
            get => this.m_nowEyewear; set => this.m_nowEyewear = value;
        }
        public CharEaring nowEaring
        {
            get => this.m_nowEaring; set => this.m_nowEaring = value;
        }
        public CharBeard nowBeard
        {
            get => this.m_nowBeard; set => this.m_nowBeard = value;
        }
        public CharHat nowHat
        {
            get => this.m_nowHat; set => this.m_nowHat = value;
        }
        public CharAura nowAura
        {
            get => this.m_nowAura; set => this.m_nowAura = value;
        }
        public List<CharItem> nowLeftHandItems
        {
            get => this.m_nowLeftHandItems; set => this.m_nowLeftHandItems = value;
        }
        public List<CharItem> nowRightHandItems
        {
            get => this.m_nowRightHandItems; set => this.m_nowRightHandItems = value;
        }
        #endregion

        #region Action prop
        public CharLoopMotion loopAction
        {
            get => this.m_loopMotion; set => this.m_loopMotion = value;
        }
        #endregion

        private void Start()
        {
            this.PreAttach();
        }

        private void LateUpdate()
        {
            this.onUpdate.Invoke(this);
        }

        #region Part handle
        private void PreAttach()
        {
            List<ICharPart> parts = new List<ICharPart>(new ICharPart[] {
                this.nowBody,
                this.nowFace,
                this.nowHair,
                this.nowEyewear,
                this.nowEaring,
                this.nowBeard,
                this.nowHat,
                this.nowAura
            });
            parts.ForEach((part) => { 
                if (part != null) part.PreAttach(this, CharPartSide.MainPart); 
            });

            parts = new List<ICharPart>(this.nowLeftHandItems);
            parts.ForEach((part) => { 
                if (part != null) part.PreAttach(this, CharPartSide.LeftHand); 
            });

            parts = new List<ICharPart>(this.nowRightHandItems);
            parts.ForEach((part) => { 
                if (part != null) part.PreAttach(this, CharPartSide.RightHand); 
            });

            this.onUpdate.Invoke(this);
            this.onLoopMotion.Invoke(this.loopAction);
        }
        #endregion

        #region Action handle
        public void SetLoopAction(CharLoopMotion actionValue)
        {
            this.loopAction = actionValue;
            this.onLoopMotion.Invoke(actionValue);
        }

        public void SetOnceAction(CharOnceMotion actionValue)
        {
            this.onOnceMotion.Invoke(actionValue);
        }
        #endregion
    }
}