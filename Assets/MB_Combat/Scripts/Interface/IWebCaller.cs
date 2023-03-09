using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    public interface IWebCaller
    {
        
    }
    public static class WebCallerExtension
    {
        public static CombatWebBridge GetWebBridge(this IWebCaller caller) => CombatWebBridge.Instance;
    }
}