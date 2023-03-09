using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public enum ShipClan
    {
        none,
        Common,
        Cursed
    }
    public struct ShipClanCode
    {
        public const string Common = "COMMON";
        public const string Cursed = "CURSED";
    }
    public class ShipClanParser
    {
        public static ShipClan FromCode(string code)
        {
            switch (code)
            {
                case ShipClanCode.Common: return ShipClan.Common;
                case ShipClanCode.Cursed: return ShipClan.Cursed;
            }
            return ShipClan.none;
        }

        public static string ToCode(ShipClan type)
        {
            switch (type)
            {
                case ShipClan.Common: return ShipClanCode.Common;
                case ShipClan.Cursed: return ShipClanCode.Cursed;
            }
            return string.Empty;
        }
    }
}