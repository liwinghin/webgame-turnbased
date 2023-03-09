using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public interface IWebBridge
    {
       
    }
    public static class WebBridgeExtension
    {
        public static bool GetSucess(this IWebBridge webBridge, JSONObject json)
        {
            var result = false;
            json.GetField(ref result, "success");
            return result;
        }

        public static JSONObject GetBody(this IWebBridge webBridge, JSONObject json)
        {
            var result = json.GetField("body");
            return result;
        }
    }
}
    
