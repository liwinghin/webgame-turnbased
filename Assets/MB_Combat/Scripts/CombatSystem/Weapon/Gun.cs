using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class Gun : MonoBehaviour
    {
        private VFXManager vfxManager
        {
            get { return VFXManager.Instance; }
        }

        private string vfxTag = null;

        private GameObject firePT = null;
        private CharacterUnit character = null;

        private void OnEnable()
        {
            if (character != null) { character.onGunAttack.AddListener(OnAttacking); }
        }

        public void OnDisable()
        {
            character.onGunAttack.RemoveListener(OnAttacking);
        }

        public void OnSetUp(CharacterUnit unit)
        {
            character = unit;
            character.onGunAttack.AddListener(OnAttacking);

            firePT = new GameObject("Fire PT");
            firePT.transform.SetParent(transform);
            firePT.transform.localPosition = new Vector3(-0.3f, -0.1f, 0);
        }

        public void OnAttacking(AnimationType type, Transform target)
        {
            if(type == AnimationType.Gun)
            {
                vfxTag = "Gun_Shoot";
                StartCoroutine(DoGunVFX(target));
            }
        }

        private IEnumerator DoGunVFX(Transform target)
        {
            yield return new WaitForSeconds(0.3f);
            Vector3 relativePos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            vfxManager.SpawnFromPool(vfxTag, firePT.transform.position, rotation);
        }
    }
}
