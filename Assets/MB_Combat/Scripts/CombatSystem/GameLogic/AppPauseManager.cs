using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class AppPauseManager : MonoBehaviour
    {
        [SerializeField] private GamePlayManager gamePlayManager;

        private bool isGamePause = false;
        private bool onFocus = true;

        private void Awake()
        {
            gamePlayManager.onGameStart.AddListener(OnGameStart);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            onFocus = hasFocus;

            AppPause();
        }

        private void AppPause()
        {
            isGamePause = !onFocus && gamePlayManager.isGameStart;

            Time.timeScale = isGamePause ? 0 : 1;
        }

        private void OnGameStart()
        {
            AppPause();
        }
    }
}