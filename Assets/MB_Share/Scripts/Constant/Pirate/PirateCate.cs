using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public enum PirateCate
    {
        Normal_1 = 0,
        Normal_2 = 1,
        Normal_3 = 2,
        Normal_4 = 3,
        Normal_5 = 4
    }
    public static class PirateCateExtension
    {
        public static int GetTierLevel(this PirateCate cateType)
        {
            switch (cateType)
            {
                case PirateCate.Normal_1: return 0;
                case PirateCate.Normal_2: return 1;
                case PirateCate.Normal_3: return 2;
                case PirateCate.Normal_4: return 3;
                case PirateCate.Normal_5: return 4;
            }
            return 0;
        }
    }
}