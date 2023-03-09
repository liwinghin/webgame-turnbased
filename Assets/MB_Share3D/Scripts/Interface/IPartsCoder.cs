using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public interface IPartsCoder
    {
        string[] partValues { get; set; }
    }
    public static class PartsCoderExtension
    {
        public static string Encode(this IPartsCoder coder)
        {
            var code = string.Empty;
            for (var i = 0; i < coder.partValues.Length; i++)
            {
                if (i == 0)
                {
                    code = coder.partValues[i];
                }
                else
                {
                    code += "-";
                    code += coder.partValues[i];
                }
            }
            return code;
        }

        public static void Decode(this IPartsCoder coder, string code)
        {
            if (string.IsNullOrEmpty(code)) return;

            var splited = code.Split('-');
            if (splited.Length != coder.partValues.Length) return;

            for (int i = 0; i < splited.Length; i++)
            {
                coder.partValues[i] = splited[i];
            }
        }
    }
}