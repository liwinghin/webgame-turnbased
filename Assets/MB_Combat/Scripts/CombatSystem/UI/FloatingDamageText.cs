using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MB.Game
{
    public class FloatingDamageText : MonoBehaviour
    {
        public Animator anim;
        public TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
            text = anim.GetComponent<TextMeshProUGUI>();
        }

        public void SetDamageText(int dmg)
        {
            text.text = dmg.ToString();
        }

        public void SetMissText()
        {
            text.text = "Miss";
        }
    }
}
