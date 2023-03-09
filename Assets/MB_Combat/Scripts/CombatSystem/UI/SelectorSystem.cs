using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class SelectorSystem : MonoBehaviour
    {
        [SerializeField] private GameObject playerSelector;
        [SerializeField] private GameObject[] enemySelector;
        [SerializeField] private Vector3 defaultPos = new Vector3(20, 20, 0);
        [SerializeField] private Vector3 offsetPos = new Vector3(0, 2.8f, 0.25f);

        private void Awake()
        {
            playerSelector.transform.position = defaultPos;
            for (int i = 0; i < enemySelector.Length; i++)
            {
                enemySelector[i].transform.position = defaultPos;
            }
        }
        public void SetEnemySelector(List<CharacterUnit> enemyList)
        {
            for (int i = 0; i < enemySelector.Length; i++)
            {
                if (enemyList[i].isAlive)
                {
                    enemySelector[i].transform.SetParent(enemyList[i].transform);
                    enemySelector[i].transform.localPosition = offsetPos;
                    enemySelector[i].SetActive(true);
                }
            }
        }

        public void DisableEnemySelector()
        {
            for (int i = 0; i < enemySelector.Length; i++)
            {
                enemySelector[i].SetActive(false);
            }
        }

        public void SetSelector(Transform target)
        {
            playerSelector.transform.SetParent(target);
            playerSelector.transform.localPosition = offsetPos;
            playerSelector.SetActive(true);
        }

        public void DisableSelector()
        {
            playerSelector.SetActive(false);
        }
    }
}
