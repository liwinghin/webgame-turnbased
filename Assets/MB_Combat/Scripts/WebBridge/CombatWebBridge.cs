using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Events;

namespace MB.Game
{
    public class CombatWebBridge : MonoBehaviour
    {
        public const string ReceiverName = "WebBridge";

        [DllImport("__Internal")]
        private static extern void RequestCombatStart();
        [DllImport("__Internal")]
        private static extern void RequestCombatClose(string message);

        public UnityEvent<bool, CombatStart> onCombatStart = new UnityEvent<bool, CombatStart>();
        public UnityEvent<bool> onCombatClose = new UnityEvent<bool>();

        public static CombatWebBridge Instance { get; private set; }

        public static CombatWebBridge CreateInstance()
        {
            var gObj = new GameObject(ReceiverName);
            DontDestroyOnLoad(gObj);
            return gObj.AddComponent<CombatWebBridge>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Instance = CreateInstance();
        }

        #region Combat Start
        public void RequestForCombatStart()
        {
#if UNITY_EDITOR
            JsonTester.RequestDebug(CombatJsonKey.CombatStart, this.RespondCombatStart);
#elif UNITY_WEBGL
            RequestCombatStart();
#endif
        }

        public void RespondCombatStart(string raw)
        {
            this.ProcessCombatStart(raw);
        }

        private void ProcessCombatStart(string raw)
        {
            var json = new JSONObject(raw);
            if (this.GetSucess(json))
            {
                var body = this.GetBody(json);
                var data = CombatStart.Decode(body);
                this.onCombatStart.Invoke(true, data);
            }
            else
            {
                this.onCombatStart.Invoke(false, null);
            }
        }
        #endregion

#region Combat Close
        public void RequestForCombatClose(string combatId, CombatResultType combatResult)
        {
#if UNITY_EDITOR
            JsonTester.RequestDebug(CombatJsonKey.CombatClose, this.RespondCombatClose);
#elif UNITY_WEBGL
            var input = this.ConstructCombatCloseInput(combatId, combatResult);
            RequestCombatClose(input.ToString());
#endif
        }

        public void RespondCombatClose(string raw)
        {
            this.ProcessCombatClose(raw);
        }

        private JSONObject ConstructCombatCloseInput(string combatId, CombatResultType combatResult)
        {
            var json = new JSONObject();

            json.AddField("combatId", combatId);
            json.AddField("combatResult", CombatResultParser.ToCode(combatResult));

            return json;
        }

        private void ProcessCombatClose(string raw)
        {
            var json = new JSONObject(raw);
            if (this.GetSucess(json))
            {
                this.onCombatClose.Invoke(true);
            }
            else
            {
                this.onCombatClose.Invoke(false);
            }
        }
#endregion

#region Process handle
        private bool GetSucess(JSONObject json)
        {
            var result = false;
            json.GetField(ref result, "success");
            return result;
        }

        private JSONObject GetBody(JSONObject json)
        {
            var result = json.GetField("body");
            return result;
        }
#endregion
    }
}