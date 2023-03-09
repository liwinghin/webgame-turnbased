using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class UICharacterIcon : MonoBehaviour
    {
        [SerializeField] RawImage characterIcon = null;
        [SerializeField] Image teamImg = null;

        public CharacterUnit characterUnit = null;
        public Text speed_text = null;
        public Text luck_text = null;

        public void ShowDebugInfo(bool active)
        {
            speed_text.SetActive(active);
            luck_text.SetActive(active);
        }

        public void DestroyIcon()
        {
            Destroy(gameObject, 2f);
            StartCoroutine(FadeInOut(0.3f));
        }

        public void ApplyCharacterInfo(CharacterUnit character, Sprite img)
        {
            characterUnit = character;
            //StartCoroutine(character.GetCharacterIcon(SetTexture));
            //teamImg.sprite = img;
            //teamImg.SetNativeSize();

            luck_text.text = "Luck: " + characterUnit.characterAbility.luck.ToString();
            speed_text.text = "Agility: " + characterUnit.characterAbility.turnSpeed.ToString();
        }

        private void SetTexture(Texture2D texture)
        {
            characterIcon.texture = texture;
        }

        IEnumerator FadeInOut(float fadeOutDelay)
        {
            Color cTemp = characterIcon.color;

            while (cTemp.a > 0)
            {
                cTemp.a -= Time.deltaTime / fadeOutDelay;

                if (cTemp.a <= 0) cTemp.a = 0;

                characterIcon.color = cTemp;
                teamImg.color = cTemp;

                yield return null;
            }
        }
    }
}
