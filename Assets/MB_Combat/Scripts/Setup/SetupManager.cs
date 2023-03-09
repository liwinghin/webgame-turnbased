using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class SetupManager : MonoBehaviour, IWebCaller, IGameCaller
    {
        [SerializeField] private string m_nextSceneName = string.Empty;

        private CombatWebBridge webBridge => this.GetWebBridge();
        private CombatContainer combatContainer => this.GetCombatContainer();

        private void OnEnable()
        {
            this.SetEvent(true);
        }

        private void Start()
        {
            StartCoroutine(this.SceneThread());
        }

        private void OnDisable()
        {
            this.SetEvent(false);
        }

        #region Set up
        private void SetEvent(bool setValue)
        {
            var webBridge = this.webBridge;
            if (webBridge == null) return;

            if (setValue)
            {
                webBridge.onCombatStart.AddListener(this.OnCombatStart);
            }
            else
            {
                webBridge.onCombatStart.RemoveListener(this.OnCombatStart);
            }
        }
        #endregion

        #region Combat handle
        private void RunCombatStart(CombatStart data)
        {
            var gameContainer = this.combatContainer;

            gameContainer.combatDetail = data.combatDetail;
            gameContainer.option = data.option;
            gameContainer.ship = data.ship;
            gameContainer.teammates = data.teammates;
            gameContainer.waves = data.waves;

            gameContainer.GoScene(this.m_nextSceneName);
        }
        #endregion

        #region Event handle
        private void OnCombatStart(bool success, CombatStart data)
        {
            if (success)
            {
                this.RunCombatStart(data);
            }
        }
        #endregion

        #region Threading
        private IEnumerator SceneThread()
        {
            yield return this.StartThread();
        }

        private IEnumerator StartThread()
        {
            // Web
            if (this.webBridge != null)
            {
                this.webBridge.RequestForCombatStart();
            }

            yield return null;
        }
#endregion
    }
}