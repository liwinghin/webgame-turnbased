using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace MB.Game
{
    public enum GameState { Init, Start, Playing, Won, Lose }

    public class GamePlayManager : MonoBehaviour, IWebCaller, IGameCaller
    {
        private CombatContainer combat => this.GetCombatContainer();

        private static GamePlayManager m_Instance = null;
        public static GamePlayManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<GamePlayManager>();
                return m_Instance;
            }
        }
        private TurnManager turnmManager
        {
            get { return TurnManager.Instance; }
        }
        private GameState gameState = GameState.Init;

        [SerializeField] private BaseFormationHandler m_pirateFormation = null;
        [SerializeField] private BaseFormationHandler m_enemyFormation = null;

        public BaseFormationHandler pirateFormation
        {
            get { return m_pirateFormation; }
            set { m_pirateFormation = value; }
        }
        public BaseFormationHandler enemyFormation
        {
            get { return m_enemyFormation; }
            set { m_enemyFormation = value; }
        }

        [Header("Env Setting")]
        public bool showDebug = false;
        [SerializeField] private CharSpawner charSpawner = null;
        [SerializeField] private UIResultPanel resultPanel = null;

        [Header("Game Event")]
        public UnityEvent onGameInit = new UnityEvent();
        public UnityEvent onGameStart = new UnityEvent();
        public UnityEvent onGameStartPlaying = new UnityEvent();

        public UnityEvent onShipAttack = new UnityEvent();
        public UnityEvent<int> onSetAction = new UnityEvent<int>();

        private CombatWebBridge webBridge => this.GetWebBridge();

        private bool m_isGameStart = false;
        public bool isGameStart
        {
            get => m_isGameStart;
            private set
            {
                m_isGameStart = value;
                if (m_isGameStart) onGameStart.Invoke();
            }
        }
        public int maxWave
        {
            get
            {
                if (combat.waves != null)
                    return combat.waves.Count;
                return 1;
            }
        }

        private void OnEnable()
        {
            resultPanel.onBackClick.AddListener(OnBackClick);
            charSpawner.onSetupDone.AddListener(OnAllCharactedLoaded);
            turnmManager.onPlayerWon.AddListener(OnPlayerWon);
            turnmManager.onPlayerLose.AddListener(OnPlayerLose);

            isGameStart = false;
        }

        private void OnDisable()
        {
            resultPanel.onBackClick.RemoveListener(OnBackClick);
            charSpawner.onSetupDone.RemoveListener(OnAllCharactedLoaded);
            turnmManager.onPlayerWon.RemoveListener(OnPlayerWon);
            turnmManager.onPlayerLose.RemoveListener(OnPlayerLose);
        }

        private void Awake()
        {
            onGameInit.Invoke();
        }

        private void OnPlayerWon()
        {
            AudioManager.PlayGameFinishedMusic(true);
            gameState = GameState.Won;
        }
        private void OnPlayerLose()
        {
            AudioManager.PlayGameFinishedMusic(false);
            gameState = GameState.Lose;
        }


        #region Start Game logic
        private void OnAllCharactedLoaded()
        {
            isGameStart = true;
            AudioManager.SetVolume(combat.option.bgmVolume, combat.option.sfxVolume);
            AudioManager.PlayBgm(0);
            gameState = GameState.Playing;
            onGameStartPlaying.Invoke();
        }
        #endregion

        #region Button logic
        public void SetPlayerAttackButton(int action)
        {
            if(gameState != GameState.Playing) { return; }
            onSetAction.Invoke(action);
        }

        public void PressedShipAttackButton()
        {
            if (gameState != GameState.Playing) { return; }
            onShipAttack.Invoke();
        }
        #endregion

        #region End game logic
        private void OnBackClick()
        {
            var combatId = combat.combatDetail.id;
            var sendResult = gameState == GameState.Won ? CombatResultType.Win : CombatResultType.Lose;
            webBridge.RequestForCombatClose(combatId, sendResult);
        }
        #endregion
    }
}
