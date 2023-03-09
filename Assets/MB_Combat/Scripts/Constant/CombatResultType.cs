using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public enum CombatResultType
    {
        Win,
        Lose
    }
    public struct CombatResultCode
    {
        public const string Win = "WIN";
        public const string Lose = "LOSE";
    }
    public class CombatResultParser
    {
        public static CombatResultType FromCode(string code)
        {
            switch (code)
            {
                case CombatResultCode.Win: return CombatResultType.Win;
                case CombatResultCode.Lose: return CombatResultType.Lose;
            }
            return CombatResultType.Lose;
        }

        public static string ToCode(CombatResultType result)
        {
            switch (result)
            {
                case CombatResultType.Win: return CombatResultCode.Win;
                case CombatResultType.Lose: return CombatResultCode.Lose;
            }
            return CombatResultCode.Lose;
        }
    }
}
