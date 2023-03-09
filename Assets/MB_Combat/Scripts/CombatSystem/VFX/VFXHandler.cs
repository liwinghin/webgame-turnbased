using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class VFXHandler : MonoBehaviour
    {
        private VFXManager vfxManager
        {
            get { return VFXManager.Instance; }
        }

        [SerializeField] private string vfxTag = string.Empty;
        [SerializeField] private float recoveryTime = 3.0f;
        [SerializeField] private ParticleSystem particle = null;

        private float _timer;

        public void initParticle()
        {
            if(particle == null) { return; }
            particle.Play();
            particle.time = 0;
            particle.Stop();
        }

        void OnEnable()
        {
            _timer = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (Time.time > _timer + recoveryTime)
            {
                vfxManager.Recovery(vfxTag, gameObject);
            }
        }
    }
}
