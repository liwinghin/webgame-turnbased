using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace MB.Game
{
    public class CombatContainer : MonoBehaviour
    {
        public CombatDetail combatDetail { get; set; }
        public CombatOption option { get; set; }
        public ShipCombat ship { get; set; }
        public List<CharacterCombat> teammates { get; set; }
        public List<WaveCombat> waves { get; set; }

        public static CombatContainer Instance { get; private set; }

        public static CombatContainer CreateInstance()
        {
            var gObj = new GameObject("Game Container");
            DontDestroyOnLoad(gObj);
            return gObj.AddComponent<CombatContainer>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Instance = CreateInstance();
        }

        #region Game handle
        public void GoScene(string sceneName)
        {
            StartCoroutine(this.LoadScene(sceneName));
        }
        #endregion

        #region Threading
        private IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        #endregion
    }
}
