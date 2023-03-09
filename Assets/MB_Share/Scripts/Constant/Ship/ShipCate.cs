using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public enum ShipCate
    {
        Normal_1 = 0,
        Normal_2 = 1,
        Cursed_1 = 5,
        Cursed_2 = 6
    }
    public static class ShipCateExtension
    {
        public static bool GetIsNormal(this ShipCate cate)
        {
            switch (cate)
            {
                case ShipCate.Normal_1: return true;
                case ShipCate.Normal_2: return true;
            }
            return false;
        }

        public static bool GetIsCursed(this ShipCate cate)
        {
            switch (cate)
            {
                case ShipCate.Cursed_1: return true;
                case ShipCate.Cursed_2: return true;
            }
            return false;
        }

        public static int GetTierLevel(this ShipCate cate)
        {
            switch (cate)
            {
                case ShipCate.Normal_1: return 0;
                case ShipCate.Normal_2: return 1;
                case ShipCate.Cursed_1: return 0;
                case ShipCate.Cursed_2: return 1;
            }
            return 0;
        }
    }
}