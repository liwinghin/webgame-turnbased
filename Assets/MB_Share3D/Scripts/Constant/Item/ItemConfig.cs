using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace MB
{
    public partial class ItemConfig
    {
        private static readonly CombineSet m_SwordSet = new CombineSet { total = 48, numPad = 3 };
        private static readonly CombineSet m_GunSet = new CombineSet { total = 18, numPad = 3 };
    }
    public partial class ItemConfig
    {
        public static List<string> GetSwordValues()
        {
            return m_SwordSet.GetEnableValues("Sword_");
        }

        public static List<string> GetGunValues()
        {
            return m_GunSet.GetEnableValues("Gun_");
        }
    }
    public partial class ItemConfig
    {
        #region Asset path
        public static string GetItemPath(string partName)
        {
            return $"Item/{partName}";
        }
        #endregion
    }
}
