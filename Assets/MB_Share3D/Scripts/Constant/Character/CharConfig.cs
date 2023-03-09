using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace MB
{
    public partial class CharConfig
    {
        private static readonly string[] m_LoopMotionKeys = {
            string.Empty,
            "die",
            "walking",
            "dashing",
            "pushing",
            "light_attacking",
            "heay_attacking",
            "hand_waving",
            "talking",
            "clapping",
            "yeahing"
        };

        private static readonly string[] m_OnceMotionKeys = {
            string.Empty,
            "get_damage",
            "jump_with_root",
            "jump_without_root",
            "drink_potion",
            "melee_attack_01",
            "melee_attack_02",
            "melee_attack_03",
            "spear_melee_attack_01",
            "spear_melee_attack_02",
            "jump_attack_01",
            "left_cross_shoot_attack_01",
            "right_cross_shoot_attack_01",
            "gun_shoot_attack_01",
            "th_shoot_attack_01",
            "throw_01"
        };
    }
    public partial class CharConfig
    {
        public static string[] GetLoopMotionKeys()
        {
            return m_LoopMotionKeys;
        }

        public static string GetLoopMotionKey(CharLoopMotion motionValue)
        {
            var index = (int)motionValue;
            if (index < 0 || index >= m_LoopMotionKeys.Length)
                return string.Empty;
            return m_LoopMotionKeys[index];
        }

        public static string[] GetOnceMotionKeys()
        {
            return m_OnceMotionKeys;
        }

        public static string GetOnceMotionKey(CharOnceMotion motionValue)
        {
            var index = (int)motionValue;
            if (index < 0 || index >= m_OnceMotionKeys.Length)
                return string.Empty;
            return m_OnceMotionKeys[index];
        }
    }
}