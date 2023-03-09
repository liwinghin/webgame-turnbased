using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class JsonTester : MonoBehaviour
    {
#if UNITY_EDITOR
        public string key = string.Empty;
        [TextArea(8, 25)] public string debugJson = string.Empty;

        private static Dictionary<string, JsonTester> Instances = new Dictionary<string, JsonTester>();

        public static void RequestDebug(string key, Action<string> callBack)
        {
            Instances[key].RequestDebug(callBack);
        }

        private void Awake()
        {
            Instances.Add(this.key, this);
        }

        #region Send handle
        public void RequestDebug(Action<string> callBack)
        {
            StartCoroutine(this.RequestThread(callBack));
        }
        #endregion

        #region Threading
        private IEnumerator RequestThread(Action<string> callBack)
        {
            yield return new WaitForSeconds(1f);

            callBack.Invoke(this.debugJson);
        }
        #endregion
#endif
    }
}