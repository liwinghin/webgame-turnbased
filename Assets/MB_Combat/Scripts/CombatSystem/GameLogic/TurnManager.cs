using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace MB.Game
{
    public enum BattleState { Start, Player, Enemy, Won, Lose, EndTurn }
    public class TurnManager : MonoBehaviour, IWebCaller, IGameCaller
    {
        private static TurnManager m_Instance = null;
        public static TurnManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<TurnManager>();
                return m_Instance;
            }
        }

        private UIManager uiManager
        {
            get { return UIManager.Instance; }
        }
        private GamePlayManager gamePlayManager
        {
            get { return GamePlayManager.Instance; }
        }
        private ActionRunManager actionRunManager
        {
            get { return ActionRunManager.Instance; }
        }

        private BattleState battleState = BattleState.Start;
        [SerializeField] private BaseFormationHandler pirateFormation = null;
        [SerializeField] private BaseFormationHandler enemyFormation = null;

        [Header("ShipAttack - Info")]
        [SerializeField] private Transform explosionPos = null;

        [Header("Env Setting")]
        [SerializeField] private Camera cam = null;
        [SerializeField] private Vector3 pirateStartPoint = new Vector3(-10f, 0, 0);
        [SerializeField] private Vector3 enemyStartPoint = new Vector3(10f, 0, 0);
        [SerializeField] private Vector3 pirateTargetPoint = new Vector3(-2.3f, 0, 0);
        [SerializeField] private Vector3 enemyTargetPoint = new Vector3(2.3f, 0, 0);

        [Header("Time Setting")]
        [SerializeField] private float startGameDelay = 1f;
        [SerializeField] private float endTurnDelay = 2f;
        [SerializeField] private float startWaveDelay = 3f;
        [SerializeField] private float endWaveDelay = 1.5f;

        [Space]
        [Header("Game Setting")]
        [SerializeField] private bool playerAttacked = false;
        [SerializeField] private CharacterUnit currentActiveUnit = null;
        [SerializeField] private int currentWave = 0;
        [SerializeField] private int currentTurn = 0;
        [SerializeField] private List<CharacterUnit> attackQueue = new List<CharacterUnit>();
        [SerializeField] private int attackIndex = 0;

        [Header("Game Event")]
        public UnityEvent<List<CharacterUnit>, bool> onGamePlaying = new UnityEvent<List<CharacterUnit>, bool>();

        //Turn
        public UnityEvent onTurnStart = new UnityEvent();
        public UnityEvent<List<CharacterUnit>, bool> onTurnEnd = new UnityEvent<List<CharacterUnit>, bool>();
        //Player
        public UnityEvent<CharacterUnit, List<CharacterUnit>, int, int> onBeforePlayerAttack = new UnityEvent<CharacterUnit, List<CharacterUnit>, int, int>();
        public UnityEvent<CharacterUnit> onAfterPlayerAttack = new UnityEvent<CharacterUnit>();
        public UnityEvent onPlayerWon = new UnityEvent();
        public UnityEvent onPlayerLose = new UnityEvent();
        //Enemy
        public UnityEvent<CharacterUnit> onBeforeEnemyAttack = new UnityEvent<CharacterUnit>();
        public UnityEvent<CharacterUnit> onAfterEnemyAttack = new UnityEvent<CharacterUnit>();
        //Wave
        public UnityEvent<List<CharacterUnit>, bool> onWaveStart = new UnityEvent<List<CharacterUnit>, bool>();
        public UnityEvent onWaveEnd = new UnityEvent();

        private void OnEnable()
        {
            gamePlayManager.onGameStart.AddListener(OnGameStart);
            gamePlayManager.onGameStartPlaying.AddListener(OnGamePlaying);
            gamePlayManager.onShipAttack.AddListener(OnShipAttack);
            gamePlayManager.onSetAction.AddListener(OnSetAction);
        }
        private void OnDisable()
        {
            gamePlayManager.onGameStart.RemoveListener(OnGameStart);
            gamePlayManager.onGameStartPlaying.RemoveListener(OnGamePlaying);
            gamePlayManager.onShipAttack.RemoveListener(OnShipAttack);
            gamePlayManager.onSetAction.RemoveListener(OnSetAction);
        }

        private void OnGameStart()
        {
            print("Turn Manager: On Game Start");
        }

        private void OnGamePlaying()
        {
            print("Turn Manager: On Game Playing");
            StartCoroutine(StartGamePlay());
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.PlayMouseClick();
                if (battleState == BattleState.Player && currentActiveUnit != null && currentActiveUnit.isAlive)
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (!Physics.Raycast(ray, out hitInfo))
                        return;

                    var target = hitInfo.collider.GetComponent<CharacterUnit>();

                    //Move to Battle Manager
                    if (target != null && target.isAlive)
                    {
                        bool isAttacked = actionRunManager.ClickedTarget(currentActiveUnit, target);
                        playerAttacked = isAttacked;
                    }
                }
            }
        }

        #region Change Turn Logic
        private void NextTurn()
        {
            //Set Ui
            pirateFormation.CheckCharacterPosition(0);
            enemyFormation.CheckCharacterPosition(currentWave);
            onTurnStart.Invoke();

            //Is all Player or enemy die
            bool isAlive = pirateFormation.IsUnitStillAlive(0) && enemyFormation.IsUnitStillAlive(currentWave) ? true : false;

            //still both teams alive
            if (isAlive)
            {
                if (attackIndex == attackQueue.Count)
                {
                    battleState = BattleState.EndTurn;
                    StartCoroutine(EndTurn());
                }
                else if (attackIndex < attackQueue.Count)
                {
                    currentActiveUnit = null;

                    //which Team
                    if (attackQueue[attackIndex].characterAbility.unitTeam == pirateFormation.teamType)
                    {
                        currentActiveUnit = attackQueue[attackIndex];
                        PlayerTurn(currentActiveUnit);
                        attackIndex++;
                    }
                    else
                    {
                        currentActiveUnit = attackQueue[attackIndex];
                        EnemyTurn(currentActiveUnit);
                        attackIndex++;
                    }
                }
            }
            else
            {
                onWaveEnd.Invoke();
                currentWave++;

                if (!pirateFormation.IsUnitStillAlive(0))
                {
                    battleState = BattleState.Lose;
                    onPlayerLose.Invoke();

                    foreach (CharacterUnit c in enemyFormation.teamUnites[currentWave - 1].charUnites)
                    {
                        if (c.isAlive)
                            c.SetCharacterLoopAnimation(CharLoopMotion.Victory);
                    }
                }
                else
                {
                    //Is it finally Wave?
                    if (currentWave >= gamePlayManager.maxWave)
                    {
                        if (pirateFormation.IsUnitStillAlive(0))
                        {
                            battleState = BattleState.Won;
                            onPlayerWon.Invoke();

                            //PlayWinAnimation
                            foreach (CharacterUnit c in pirateFormation.teamUnites[0].charUnites)
                            {
                                if (c.isAlive)
                                    c.SetCharacterLoopAnimation(CharLoopMotion.Victory);
                            }
                        }
                    }
                    else
                    {
                        StartCoroutine(NextWave());
                    }
                }
            }
        }

        private IEnumerator EndTurn()
        {
            attackIndex = 0;
            currentTurn++;
            CharactersEnergyRecovery();
            SortAttackQueue(pirateFormation.GetCurrentUnits(0), enemyFormation.GetCurrentUnits(currentWave));
            onTurnEnd.Invoke(attackQueue, gamePlayManager.showDebug);
            yield return new WaitForSeconds(endTurnDelay);
            NextTurn();
        }
        #endregion

        #region Start Game logic
        private void InitGameEnv()
        {
            battleState = BattleState.Start;
            currentWave = 0;
            currentTurn = 0;
            SetWaveEnemy();
        }

        private IEnumerator StartGamePlay()
        {
            InitGameEnv();
            SortAttackQueue(pirateFormation.GetCurrentUnits(0), enemyFormation.GetCurrentUnits(currentWave));
            StartCoroutine(Move(pirateFormation, pirateStartPoint, pirateTargetPoint, 0, 0.8f));
            StartCoroutine(Move(enemyFormation, enemyStartPoint, enemyTargetPoint, currentWave, 0.8f));
            yield return new WaitForSeconds(startGameDelay);
            //OnGameStart UI Setting
            onGamePlaying.Invoke(attackQueue, gamePlayManager.showDebug);     
            NextTurn();
        }
        #endregion

        #region Wave Setup logic
        IEnumerator NextWave()
        {
            CharactersEnergyRecovery();
            yield return new WaitForSeconds(endWaveDelay);
            //Disable dead pirate and all Enemy
            pirateFormation.CheckCharacterAlive(0);
            enemyFormation.SetWaveEnemyActive(-1);
            currentActiveUnit = null;
            yield return new WaitForSeconds(startWaveDelay);
            SetWaveEnemy();
            yield return Move(enemyFormation, enemyStartPoint, enemyTargetPoint, currentWave, 1f);
            SortAttackQueue(pirateFormation.GetCurrentUnits(0), enemyFormation.GetCurrentUnits(currentWave));
            onWaveStart.Invoke(attackQueue, gamePlayManager.showDebug);
            attackIndex = 0;
            currentTurn++;
            NextTurn();
        }
        private void SetWaveEnemy()
        {
            uiManager.SetWaveText(currentWave, gamePlayManager.maxWave);
            enemyFormation.SetWaveEnemyActive(currentWave);
        }

        private void SortAttackQueue(List<CharacterUnit> player, List<CharacterUnit> pirate)
        {
            attackQueue.Clear();
            attackQueue = player.Union(pirate).ToList();
            attackQueue.Sort(new UnitComparer());
        }

        private void BattleTurnSetup(CharacterUnit unit, BattleState state)
        {
            battleState = state;
            currentActiveUnit = unit;
            currentActiveUnit.SetCurrentAction(0);
        }

        private void CharactersEnergyRecovery()
        {
            pirateFormation.SetCharacterEnergy(0);
            enemyFormation.SetCharacterEnergy(currentWave);
        }

        #endregion

        #region Player logic
        private void PlayerTurn(CharacterUnit currentUnit)
        {
            BattleTurnSetup(currentUnit, BattleState.Player);
            playerAttacked = false;
            StartCoroutine(PlayerAttack());
        }

        private IEnumerator PlayerAttack()
        {
            if (currentActiveUnit.isAlive == true)
            {
                onBeforePlayerAttack.Invoke(currentActiveUnit, enemyFormation.teamUnites[currentWave].charUnites, currentTurn, pirateFormation.shipInfo.cannonTurn);

                while (true)
                {
                    if (playerAttacked)
                    {
                        onAfterPlayerAttack.Invoke(currentActiveUnit);
                        yield return new WaitUntil(() => uiManager.isDeleting == false);
                        yield return new WaitUntil(() => currentActiveUnit.IsDoingAction == false);
                        NextTurn();
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                onAfterPlayerAttack.Invoke(currentActiveUnit);
                yield return new WaitUntil(() => uiManager.isDeleting == false);
                NextTurn();
            }
        }

        private void OnSetAction(int action)
        {
            currentActiveUnit.SetCurrentAction(action);
        }

        private void OnShipAttack()
        {
            StartCoroutine(ShipAttack());
        }

        private IEnumerator ShipAttack()
        {
            currentTurn = 0;
            actionRunManager.ShipAttacked(currentActiveUnit, enemyFormation.teamUnites[currentWave].charUnites, explosionPos);
            yield return new WaitUntil(() => currentActiveUnit.IsDoingAction == false);
            playerAttacked = true;
        }
        #endregion

        #region Enemy logic
        private void EnemyTurn(CharacterUnit currentUnit)
        {
            BattleTurnSetup(currentUnit, BattleState.Enemy);
            StartCoroutine(EnemyAttack());
        }

        IEnumerator EnemyAttack()
        {
            yield return new WaitForSeconds(0.5f);

            if (currentActiveUnit.isAlive == true)
            {
                onBeforeEnemyAttack.Invoke(currentActiveUnit);
                yield return new WaitForSeconds(0.5f);
                onAfterEnemyAttack.Invoke(currentActiveUnit);
                actionRunManager.RandomEnemyAndSkill(currentActiveUnit, pirateFormation, 0);
                yield return new WaitUntil(() => uiManager.isDeleting == false);
                yield return new WaitUntil(() => currentActiveUnit.IsDoingAction == false);
                NextTurn();
            }
            else
            {
                onAfterEnemyAttack.Invoke(currentActiveUnit);
                yield return new WaitUntil(() => uiManager.isDeleting == false);
                NextTurn();
            }
        }
        #endregion

        private IEnumerator Move(BaseFormationHandler handler, Vector3 startPos, Vector3 targetPos, int currentWave, float seconds)
        {
            float elapsedTime = 0;
            handler.transform.position = startPos;
            handler.SetCharacterAnimation(currentWave, CharLoopMotion.Running);

            while (elapsedTime < seconds)
            {
                handler.transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            handler.transform.position = targetPos;
            handler.SetCharacterAnimation(currentWave, CharLoopMotion.Idle);
        }
    }
}
