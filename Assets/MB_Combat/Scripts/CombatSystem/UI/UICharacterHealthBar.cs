using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MB.Game
{
    public class UICharacterHealthBar : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [Header("HP")]
        [SerializeField] private Image hpBar;
        [SerializeField] private TextMeshProUGUI hpText;
        [Header("EN")]
        [SerializeField] private Image enBar;
        [SerializeField] private TextMeshProUGUI enText;
        [SerializeField] private Sprite[] starsImg;
        [SerializeField] private Image tierImg;

        [SerializeField] private float reduceSpeed = 0.75f;
        [SerializeField] private RectTransform rect;
        public CharacterUnit characterReference = null;

        private float targetHP = 1;
        private float targetEN = 0;

        private void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (characterReference != null)
                rect.transform.position = mainCam.WorldToScreenPoint(characterReference.uiContainer.transform.position);
        }

        public void SetCharacter(CharacterUnit reference)
        {
            characterReference = reference;
            if(reference != null) {
                tierImg.sprite = starsImg[reference.characterAbility.tier - 1];
                tierImg.SetNativeSize();
                SetInfoText(reference.characterAbility.currentHp, reference.characterAbility.currentEnergy);
            }
            else { rect.transform.position = new Vector3(-200, -200, -200); }
        }

        private void SetInfoText(float currentHP, float currentEN)
        {
            string hp = currentHP.ToString();
            if(currentHP < 0) { hp = "0"; }
            string en = currentEN.ToString();
            if (currentEN < 0) { en = "0"; }
            hpText.text = hp;
            enText.text = en;
        }

        public void UpdateCharacterBar(float currentHP, float maxHP, float currentEN, float maxEN, bool init)
        {
            targetHP = currentHP / maxHP;
            targetEN = currentEN / maxEN;

            if (init)
            {
                hpBar.fillAmount = targetHP;
                enBar.fillAmount = targetEN;
            }
            else
            {
                StartCoroutine(UpdateBarUI(targetHP, targetEN));
                SetInfoText(currentHP, currentEN);
            }
        }

        private IEnumerator UpdateBarUI(float hp, float en)
        {
            float currentHP = hpBar.fillAmount;
            float currentEN = enBar.fillAmount;

            float elapsed = 0f;

            while (elapsed < reduceSpeed)
            {
                elapsed += Time.deltaTime;
                hpBar.fillAmount = Mathf.Lerp(currentHP, hp, elapsed / reduceSpeed);
                enBar.fillAmount = Mathf.Lerp(currentEN, en, elapsed / reduceSpeed);
                yield return null;
            }
            hpBar.fillAmount = hp;
            enBar.fillAmount = en;
        }

        private void Update()
        {
            if (characterReference != null)
            {
                rect.transform.position = mainCam.WorldToScreenPoint(characterReference.uiContainer.transform.position);
            }
        }
    }
}
