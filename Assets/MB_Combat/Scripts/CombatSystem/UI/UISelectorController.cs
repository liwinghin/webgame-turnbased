using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class UISelectorController : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] private Image playerSelector;
        [SerializeField] private Image[] enemySelector;

        [SerializeField] private Vector2 offsetPos = new Vector2(0, 120f);
        private Vector3 initPos = Vector3.zero;

        private CharacterUnit currentUnit = null;
        private List<CharacterUnit> currentList = null;

        private bool showcurrentUnitSelector = false;
        private bool showcurrentEnemysSelector = false;

        private void Awake()
        {
            initPos = playerSelector.transform.position;
            showcurrentUnitSelector = false;
            showcurrentEnemysSelector = false;
        }

        public void SetPlayerSelector(CharacterUnit player)
        {
            showcurrentUnitSelector = true;
            currentUnit = player;
            SetPlayerPosition();
        }

        public void SetPlayerPosition()
        {
            if(currentUnit == null) { return; }
            Vector3 newPos = mainCam.WorldToScreenPoint(currentUnit.uiContainer.transform.position);
            playerSelector.rectTransform.position = newPos;
            playerSelector.rectTransform.anchoredPosition += offsetPos;
        }
       
        public void SetEnemySelector(List<CharacterUnit> enemyList)
        {
            showcurrentEnemysSelector = true;
            currentList = enemyList;
            SetEnemySelectorPosition();
        }

        public void SetEnemySelectorPosition()
        {
            if (currentList == null) { return; }
            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i].isAlive)
                {
                    Vector3 newPos = mainCam.WorldToScreenPoint(currentList[i].uiContainer.transform.position);
                    enemySelector[i].rectTransform.position = newPos;
                    enemySelector[i].rectTransform.anchoredPosition += offsetPos;
                }
            }
        }

        public void DisableSelector()
        {
            showcurrentUnitSelector = false;
            showcurrentEnemysSelector = false;

            playerSelector.rectTransform.position = initPos;

            for (int i = 0; i < enemySelector.Length; i++)
            {
                enemySelector[i].rectTransform.position = initPos;
            }
        }
        public void LateUpdate()
        {
            if (showcurrentUnitSelector)
            {
                SetPlayerPosition();
            }
            if (showcurrentEnemysSelector)
            {
                SetEnemySelectorPosition();
            }
        }
    }
}
