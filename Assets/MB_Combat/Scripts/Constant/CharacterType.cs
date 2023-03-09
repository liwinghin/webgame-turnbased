using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    public enum CharacterType
    {
        none,
        Pirate
    }
    public struct CharacterTypeCode
    {
        public const string Pirate = "PIRATE";
    }
    public class CharacterTypeParser
    {
        public static CharacterType FromCode(string code)
        {
            switch (code)
            {
                case CharacterTypeCode.Pirate: return CharacterType.Pirate;
            }
            return CharacterType.none;
        }
    }
}
