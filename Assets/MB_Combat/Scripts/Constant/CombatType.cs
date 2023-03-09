using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    public enum CombatType
    {
        Robbery
    }
    public struct CombatTypeCode
    {
        public const string Robbery = "ROBBERY";
    }
    public class CombatTypeParser
    {
        public static CombatType FromCode(string code)
        {
            switch (code)
            {
                case CombatTypeCode.Robbery: return CombatType.Robbery;
            }
            return CombatType.Robbery;
        }
    }
}