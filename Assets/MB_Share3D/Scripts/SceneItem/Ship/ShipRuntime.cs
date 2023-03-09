using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace MB
{
    public class ShipRuntime : MonoBehaviour
    {
        [Header("Parts")]
        [SerializeField] private ShipBody m_nowBody = null;

        [Header("Event")]
        public UnityEvent<ShipRuntime> onUpdate = new UnityEvent<ShipRuntime>();

        #region Part prop
        public ShipBody nowBody
        {
            get => this.m_nowBody; set => this.m_nowBody = value;
        }
        public IShipPart[] allParts
        {
            get => new IShipPart[] {
                this.nowBody
            };
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
            foreach (IShipPart part in this.allParts)
            {
                if (part != null)
                {
                    part.PreAttach(this);
                }
            }
            this.onUpdate.Invoke(this);
        }
        #endregion
    }
}