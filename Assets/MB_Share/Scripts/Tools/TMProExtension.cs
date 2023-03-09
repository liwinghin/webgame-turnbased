using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace MB
{
    public static class TMProExtension
    {
        public static void SetHeading1(this TMP_Text tm, string text)
        {
            tm.text = $"<style=\"H1\">{text}</style>";
        }

        public static void SetHeading3(this TMP_Text tm, string text)
        {
            tm.text = $"<style=\"H3\">{text}</style>";
        }

        public static void SetTag(this TMP_Text tm, string text)
        {
            tm.text = $"<style=\"TAG\">{text}</style>";
        }
    }
}