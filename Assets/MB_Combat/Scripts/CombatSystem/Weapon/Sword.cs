using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class Sword : MonoBehaviour
    {
        private VFXManager vfxManager
        {
            get { return VFXManager.Instance; }
        }

        private string vfxTag = null;

        private CharacterUnit character = null;

        private void OnEnable()
        {
            if (character != null) { character.onSwordAttack.AddListener(OnAttacking); }
        }

        public void OnDisable()
        {
            character.onSwordAttack.RemoveListener(OnAttacking);
        }

        public void OnSetUp(CharacterUnit unit)
        {
            character = unit;
            character.onSwordAttack.AddListener(OnAttacking);
        }

        public void OnAttacking(CharOnceMotion onceMotion, Transform target)
        {
            switch (onceMotion)
            {
                case CharOnceMotion.MeleeAttack1:
                    vfxTag = "Sword_Attack";
                    break;

                case CharOnceMotion.JumpAttack:
                    vfxTag = "Sword_HeavyAttack";
                    break;
            }
            StartCoroutine(DoSword(target));
        }

        private IEnumerator DoSword(Transform target)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 relativePos = target.position - character.transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            vfxManager.SpawnFromPool(vfxTag, transform.position, rotation);
        }
    }
}
