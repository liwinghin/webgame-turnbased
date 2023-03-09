using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace MB
{
    public partial class ShipConfig
    {
        public const string DefaultValue = "000";

        public const string Normal1Body = "000";
        public const string Normal2Body = "001";
        public const string Cursed1Body = "C001";
        public const string Cursed2Body = "C001";

        private static readonly CombineSet[] m_NormalCombines = {
            new CombineSet { total = 2, numPad = 3 },
            new CombineSet { total = 4, numPad = 3 },
            new CombineSet { total = 16, numPad = 3 },
            new CombineSet { total = 1, numPad = 3 }
        };

        private static readonly bool[] m_DefaultNull = {
            false,
            false,
            false,
            false,
            true,
            true,
            true
        };

        private static readonly string[] m_CateBody = {
            "000",
            "001",
            string.Empty,
            string.Empty,
            string.Empty,
            "C000",
            "C001",
            string.Empty,
            string.Empty,
            string.Empty
        };

        private static readonly string[] m_CateBackground = {
            "000",
            "000",
            "000",
            "000",
            "000",
            "000",
            "000",
            "000",
            "000",
            "000"
        };
    }
    public partial class ShipConfig
    {
        public static List<string> GetNormalValues(ShipValue key)
        {
            var index = (int)key;
            return m_NormalCombines[index].GetEnableValues();
        }

        public static bool GetBodyNull(ShipCombine combine) => GetNull(combine, ShipValue.Body);
        public static bool GetBodySkinNull(ShipCombine combine) => GetNull(combine, ShipValue.Skin);
        public static bool GetSailSkinNull(ShipCombine combine) => GetNull(combine, ShipValue.Sail);
        public static bool GetBgNull(ShipCombine combine) => GetNull(combine, ShipValue.Background);

        public static string GetCateBody(ShipCate cate)
        {
            var index = (int)cate;
            return m_CateBody[index];
        }

        public static string GetCateBackground(ShipCate cate)
        {
            var index = (int)cate;
            return m_CateBackground[index];
        }

        private static bool GetNull(ShipCombine combine, ShipValue key)
        {
            var index = (int)key;

            if (combine.partValues[index] == string.Empty)
            {
                return true;
            }

            if (combine.partValues[index] == DefaultValue)
            {
                return m_DefaultNull[index];
            }

            return false;
        }
    }
    public partial class ShipConfig
    {
        #region File path
        public static string GetFileDirectory()
        {
            return $"{Application.persistentDataPath}/Ship";
        }

        public static string GetMediaDirectory(ShipCate cate, string code)
        {
            var tierLv = cate.GetTierLevel();
            var path = new string[] {
                GetFileDirectory(),
                $"tier_{tierLv + 1}",
                code
            };
            return Path.Combine(path);
        }

        public static string GetGenJsonName(ShipCate cate)
        {
            var tierLv = cate.GetTierLevel();

            if (cate.GetIsNormal())
            {
                return $"tier_{tierLv + 1}.json";
            }
            if (cate.GetIsCursed())
            {
                return $"c_tier_{tierLv + 1}.json";
            }

            return string.Empty;
        }

        public static string GetCapturedJsonName()
        {
            return $"captured.json";
        }
        #endregion

        #region Asset path
        public static string GetBodyPath(ShipCombine combine)
        {
            return $"ShipBody/{combine.bodyName}";
        }
        public static string GetBodySkinPath(ShipCombine combine)
        {
            return $"ShipSkin/{combine.bodySkinName}";
        }
        public static string GetSailSkinPath(ShipCombine combine)
        {
            return $"ShipSail/{combine.sailSkinName}";
        }
        public static string GetProductBgPath(ShipCombine combine)
        {
            return $"ShipProductBg/{combine.productBgName}";
        }
        public static string GetDetailBgPath(ShipCombine combine)
        {
            return $"ShipDetailBg/{combine.detailBgName}";
        }
        #endregion
    }
}
