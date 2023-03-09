using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class UIActionBtnHandler : MonoBehaviour
    {
        [SerializeField] private CanvasGroup actionCanvasGroup = null;

        [SerializeField] private Image selectedUI = null;
        [SerializeField] private Vector2 selectedUIOffset = new Vector2(0, 20);

        [SerializeField] private Button[] actionBtn = null;
        [SerializeField] private Image[] actionBar = null;


        public void SetSelectedUIPosition(int index)
        {
            selectedUI.rectTransform.anchoredPosition = actionBar[index].rectTransform.anchoredPosition + selectedUIOffset;
        }

        public void SetSelectedUIActive(bool active)
        {
            selectedUI.SetActive(active);
        }

        public void SetActionCanvasGroup(bool active)
        {
            actionCanvasGroup.interactable = active;
            actionCanvasGroup.alpha = (actionCanvasGroup.interactable) ? 100 : 20;
        }

        public void SetActionBtn(CharacterUnit character)
        {
            for (int i = 0; i < actionBtn.Length; i++)
            {
                bool haveBtn = character.CheckActionSetCount(i);
                bool actionAvailable = character.ActionAvailable(i);
                if (haveBtn) { SetActionBarFillAmount(i, character.characterAbility.currentEnergy, character.actionSet[i].actionEnergy); }
                SetActionBtnActive(i, haveBtn, actionAvailable);
            }
        }

        private void SetActionBarFillAmount(int bar, float currentEN, float actionEn)
        {
            if(bar == 0) { return; }
            float ActionTargetAmount = currentEN / actionEn;
            actionBar[bar].fillAmount = ActionTargetAmount;
        }

        public void SetActionBtnActive(int btn, bool haveBtn, bool active)
        {
            actionBtn[btn].interactable = active;
            actionBar[btn].SetActive(haveBtn);
        }

        public void SetActionBtnImage(CharacterUnit unit)
        {
            for (int i = 0; i < unit.actionSet.Count; i++)
            {
                actionBtn[i].image.sprite = unit.actionSet[i].actionData.actionIcon;
                actionBtn[i].image.SetNativeSize();
            }
        }

        public void DisableAllActionBtn()
        {
            for (int i = 0; i < actionBtn.Length; i++)
            {
                SetActionBtnActive(i, false, false);
            }
        }
    }
}
