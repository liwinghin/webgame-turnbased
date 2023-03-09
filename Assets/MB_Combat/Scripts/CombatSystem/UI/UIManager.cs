using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager m_Instance = null;
        public static UIManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<UIManager>();
                return m_Instance;
            }
        }
        private GamePlayManager gamePlayManager
        {
            get { return GamePlayManager.Instance; }
        }
        private TurnManager turnmManager
        {
            get { return TurnManager.Instance; }
        }

        [Header("Env UI")]
        [SerializeField] private GameObject loadingPage = null;
        [SerializeField] private Text waveText = null;

        [Header("Character UI")]
        [SerializeField] private UICharacterHealthBar[] charactersBarUI = null;
        [SerializeField] private UIActionQueue uiActionQueue = null;

        [Header("Skill UI")]
        [SerializeField] private Button shipAttackBtn = null;
        [SerializeField] private UIActionBtnHandler actionHandler = null;

        public bool isDeleting
        {
            get { return uiActionQueue.isDeleting; }
        }
        [Header("Selector UI")]
        [SerializeField] private UISelectorController selectorController = null;

        [Header("Result UI")]
        [SerializeField] private UIResultPanel resultPage = null;
        [SerializeField] private Image resultImg = null;
        [SerializeField] private Sprite[] resultSprite = null;

        private void OnEnable()
        {
            gamePlayManager.onGameInit.AddListener(OnGameInit);
            gamePlayManager.onGameStart.AddListener(OnGameStart);
            turnmManager.onGamePlaying.AddListener(OnGameStartPlaying);
            //Turn
            turnmManager.onTurnStart.AddListener(OnTurnStart);
            turnmManager.onTurnEnd.AddListener(OnTurnEnd);
            //Wave
            turnmManager.onWaveStart.AddListener(OnGameStartPlaying);
            turnmManager.onWaveEnd.AddListener(OnWaveEnd);
            //Player
            turnmManager.onBeforePlayerAttack.AddListener(BeforePlayerAttacked);
            turnmManager.onAfterPlayerAttack.AddListener(AfterPlayerAttacked);
            turnmManager.onPlayerWon.AddListener(OnPlayerWon);
            turnmManager.onPlayerLose.AddListener(OnPlayerLose);
            //Enemy
            turnmManager.onBeforeEnemyAttack.AddListener(BeforeEnemyAttacked);
            turnmManager.onAfterEnemyAttack.AddListener(AfterEnemyAttacked);
            //Action
            gamePlayManager.onShipAttack.AddListener(OnShipAttack);
            gamePlayManager.onSetAction.AddListener(OnSetAction);
        }
        private void OnDisable()
        {
            gamePlayManager.onGameInit.RemoveListener(OnGameInit);
            gamePlayManager.onGameStart.RemoveListener(OnGameStart);
            turnmManager.onGamePlaying.RemoveListener(OnGameStartPlaying);
            //Turn
            turnmManager.onTurnStart.RemoveListener(OnTurnStart);
            turnmManager.onTurnEnd.RemoveListener(OnTurnEnd);
            //Wave
            turnmManager.onWaveStart.RemoveListener(OnGameStartPlaying);
            turnmManager.onWaveEnd.RemoveListener(OnWaveEnd);
            //Player
            turnmManager.onBeforePlayerAttack.RemoveListener(BeforePlayerAttacked);
            turnmManager.onAfterPlayerAttack.RemoveListener(AfterPlayerAttacked);
            turnmManager.onPlayerWon.RemoveListener(OnPlayerWon);
            turnmManager.onPlayerLose.RemoveListener(OnPlayerLose);
            //Enemy
            turnmManager.onBeforeEnemyAttack.RemoveListener(BeforeEnemyAttacked);
            turnmManager.onAfterPlayerAttack.RemoveListener(AfterEnemyAttacked);
            //Action
            gamePlayManager.onShipAttack.RemoveListener(OnShipAttack);
            gamePlayManager.onSetAction.RemoveListener(OnSetAction);
        }
        private void OnGameStart()
        {
            print("On Game Start");
            SetLoadingPageActive(false);
            SetResultPageActive(false);
        }

        private void OnGameInit()
        {
            print("On Game Init");
            SetLoadingPageActive(true);
        }

        private void OnSetAction(int action)
        {
            SetSelectedUIPosition(action);
        }
        private void OnShipAttack()
        {
            SetShipAttackBtnActive(false);
            DisableSelector();
        }
        private void OnGameStartPlaying(List<CharacterUnit> charactersQueue, bool showDebug)
        {
            SetAttackQueueIconUI(charactersQueue, showDebug);
            SetCharactersBarUI(charactersQueue);
        }
        private void OnWaveEnd()
        {
            SetHPBarUIActive(false);
            DestroyAllCharacterIcon();
        }
        private void OnTurnStart()
        {
            SetHPBarUIActive(false);
            SetBtnInteractable(false);
            DisableAllButton();
            DisableSelector();
        }
        private void OnTurnEnd(List<CharacterUnit> charactersQueue, bool showDebug)
        {
            SetAttackQueueIconUI(charactersQueue, showDebug);
        }
        //Enemy 
        public void BeforeEnemyAttacked(CharacterUnit currentActiveUnit)
        {
            //SetSelector(currentActiveUnit);
        }
        public void AfterEnemyAttacked(CharacterUnit currentActiveUnit)
        {
            DisableSelector();
            SetCharacterIconDisable(currentActiveUnit);
            SetHPBarUIActive(false);
        }
        //Player
        private void OnPlayerWon()
        {
            SetResultPageImg(1, true);
        }
        private void OnPlayerLose()
        {
            SetResultPageImg(0, true);
        }

        public void BeforePlayerAttacked(CharacterUnit currentActiveUnit, List<CharacterUnit> enemyTeam, int currentTurn, int cannonTurn)
        {
            SetSelectedUIPosition(0);
            SetSelector(currentActiveUnit, enemyTeam);
            CheckActionBtn(currentActiveUnit, currentTurn, cannonTurn);
            SetActionBtnImage(currentActiveUnit);
            SetHPBarUIActive(true);
            SetBtnInteractable(true);
        }

        public void AfterPlayerAttacked(CharacterUnit currentActiveUnit)
        {
            SetCharacterIconDisable(currentActiveUnit);
            SetBtnInteractable(false);
            SetHPBarUIActive(false);
            DisableSelector();
        }
        //Loading Page
        public void SetLoadingPageActive(bool active)
        {
            loadingPage.SetActive(active);
        }

        //Wave Text
        public void SetWaveText(int currentWave, int maxWave)
        {
            waveText.text = "Wave " + (currentWave + 1).ToString() + " / " + maxWave.ToString();
        }
        //UI Queue
        public void DestroyAllCharacterIcon()
        {
            uiActionQueue.DestroyAllIcon();
        }

        public void SetCharacterIconDisable(CharacterUnit character)
        {
            uiActionQueue.SetIconDisable(character);
        }

        public void SetAttackQueueIconUI(List<CharacterUnit> characters, bool showDebug)
        {
            uiActionQueue.AddCharacterIcon(characters);
            ShowUIQueueDebug(showDebug);
        }

        //Result Page
        public void SetResultPageActive(bool active)
        {
            resultPage.SetActive(active);
        }

        public void SetResultPageImg(int index, bool active)
        {
            resultImg.sprite = resultSprite[index];
            resultPage.SetActive(active);
        }

        //Character HP Bar
        public void SetHPBarUIActive(bool active)
        {
            for (int i = 0; i < charactersBarUI.Length; i++)
            {
                charactersBarUI[i].SetActive(active);
            }
        }

        public void SetCharactersBarUI(List<CharacterUnit> characters)
        {
            for (int i = 0; i < charactersBarUI.Length; i++)
            {
                if (!characters.Contains(charactersBarUI[i].characterReference))
                {
                    CharacterUnit c = characters.Find((character) => character.uICharacterHealthBars == null);
                    if (c != null && i < characters.Count)
                    {
                        charactersBarUI[i].SetCharacter(c);
                        c.uICharacterHealthBars = charactersBarUI[i];
                        c.UpdateCharacterBars(true);
                    }
                    else
                    {
                        charactersBarUI[i].SetCharacter(null);
                    }
                }
            }
        }

        //Player Skill button
        public void CheckActionBtn(CharacterUnit character, int currentTurn, int shipTurn)
        {
            actionHandler.SetActionBtn(character);
            SetShipAttackBtnActive((currentTurn > shipTurn) ? true : false);
            SetSelectedUIActive(true);
        }

        public void SetBtnInteractable(bool active)
        {
            shipAttackBtn.interactable = active;
            actionHandler.SetActionCanvasGroup(active);
        }

        public void SetActionBtnImage(CharacterUnit character)
        {
            actionHandler.SetActionBtnImage(character);
            shipAttackBtn.image.sprite = character.shipAttackInfo.actionData.actionIcon;
            shipAttackBtn.image.SetNativeSize();
        }

        public void SetSelectedUIPosition(int btnPos)
        {
            actionHandler.SetSelectedUIPosition(btnPos);
        }

        public void SetSelectedUIActive(bool active)
        {
            actionHandler.SetSelectedUIActive(active);
        }

        public void SetShipAttackBtnActive(bool active)
        {
            shipAttackBtn.SetActive(active);
        }

        public void DisableAllButton()
        {
            actionHandler.DisableAllActionBtn();
            SetShipAttackBtnActive(false);
            SetSelectedUIActive(false);
        }

        //Selector
         public void SetSelector(CharacterUnit currentUnit)
        {
            selectorController.SetPlayerSelector(currentUnit);
        }

        public void SetSelector(CharacterUnit currentUnit, List<CharacterUnit> enemyList)
        {
            selectorController.SetPlayerSelector(currentUnit);
            selectorController.SetEnemySelector(enemyList);
        }

        public void DisableSelector()
        {
            selectorController.DisableSelector();
        }

        //Debug
        public void ShowUIQueueDebug(bool showDebug)
        {
            uiActionQueue.ShowDebug(showDebug);
        }
    }
}
