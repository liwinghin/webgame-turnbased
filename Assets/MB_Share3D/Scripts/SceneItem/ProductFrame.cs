using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB
{
    public class ProductFrame : MonoBehaviour
    {
        [SerializeField] private float m_defaultPitch = 0f;
        [SerializeField] private float m_defaultYaw = 0f;

        [Header("Frame")]
        [SerializeField] private Transform m_pitchRoot = null;
        [SerializeField] private Transform m_yawRoot = null;

        #region Frame
        public void ForceView()
        {
            this.ForceView(this.m_defaultPitch, this.m_defaultYaw);
        }
        public void ForceView(float xValue, float yValue)
        {
            this.m_pitchRoot.localRotation = Quaternion.Euler(xValue, 0f, 0f);
            this.m_yawRoot.localRotation = Quaternion.Euler(0f, yValue, 0f);
        }

        public void Yawing(float yawDelta)
        {
            this.m_yawRoot.Rotate(new Vector3(0f, -yawDelta, 0f) * Time.deltaTime);
        }

        public void Pitching(float pitchDelta)
        {
            var target = 0f;
            var delta = pitchDelta;
            if (pitchDelta > 0f)
                target = 90f;
            if (pitchDelta < 0f)
            {
                target = -10f;
                delta = -pitchDelta;
            }
            var rot = Quaternion.Euler(target, 0f, 0f);
            this.m_pitchRoot.localRotation = Quaternion.Slerp(this.m_pitchRoot.localRotation, rot, delta * Time.deltaTime);
        }
        #endregion
    }
}