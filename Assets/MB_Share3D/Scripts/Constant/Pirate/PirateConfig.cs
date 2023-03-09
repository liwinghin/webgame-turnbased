using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace MB
{
    public partial class PirateConfig
    {
        public const string DefaultValue = "000";

        private static readonly CombineSet[] m_CombineSet = {
            new CombineSet { total = 2, numPad = 3 },
            new CombineSet { total = 120, numPad = 3 },
            new CombineSet { total = 18, numPad = 3 },
            new CombineSet { total = 126, numPad = 3 },
            new CombineSet { total = 19, numPad = 3 },
            new CombineSet { total = 22, numPad = 3 },
            new CombineSet { total = 27, numPad = 3 },
            new CombineSet { total = 75, numPad = 3 },
            new CombineSet { total = 3, numPad = 3 },
            new CombineSet { total = 11, numPad = 3 },
            new CombineSet { total = 6, numPad = 3 },
        };

        private static readonly bool[] m_DefaultNull = {
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            false,
            true,
            true
        };
    }
    public partial class PirateConfig
    {
        public static List<string> GetEnableValues(PirateValue key)
        {
            var index = (int)key;
            return m_CombineSet[index].GetEnableValues();
        }

        public static bool GetBodyNull(PirateCombine combine) => GetNull(combine, PirateValue.Body);
        public static bool GetBodySkinNull(PirateCombine combine) => GetNull(combine, PirateValue.Skin);
        public static bool GetFaceNull(PirateCombine combine) => GetNull(combine, PirateValue.Face);
        public static bool GetHairNull(PirateCombine combine) => GetNull(combine, PirateValue.Hair);
        public static bool GetEaringNull(PirateCombine combine) => GetNull(combine, PirateValue.Earing);
        public static bool GetBeardNull(PirateCombine combine) => GetNull(combine, PirateValue.Beard);
        public static bool GetEyewearNull(PirateCombine combine) => GetNull(combine, PirateValue.Eyewear);
        public static bool GetHatNull(PirateCombine combine) => GetNull(combine, PirateValue.Hat);
        public static bool GetBgNull(PirateCombine combine) => GetNull(combine, PirateValue.Background);
        public static bool GetEnegryNull(PirateCombine combine) => GetNull(combine, PirateValue.Energy);
        public static bool GetAruaNull(PirateCombine combine) => GetNull(combine, PirateValue.Aura);

        private static bool GetNull(PirateCombine combine, PirateValue key)
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
    public partial class PirateConfig
    {
        #region File path
        public static string GetFileDirectory()
        {
            return $"{Application.persistentDataPath}/PirateCharacter";
        }

        public static string GetMediaDirectory(int tierindex, string code)
        {
            var path = new string[] {
            GetFileDirectory(),
            $"tier_{tierindex + 1}",
            code
        };
            return Path.Combine(path);
        }

        public static string GetTierJsonName(int tierindex)
        {
            return $"tier_{tierindex + 1}.json";
        }

        public static string GetCapturedJsonName()
        {
            return $"captured.json";
        }
        #endregion

        #region Asset path
        public static string GetBodyPath(PirateCombine combine)
        {
            return $"PirateBody/{combine.bodyName}";
        }
        public static string GetBodySkinPath(PirateCombine combine)
        {
            return $"PirateSkin/{combine.bodySkinName}";
        }
        public static string GetFacePath(PirateCombine combine)
        {
            return $"PirateFace/{combine.faceName}";
        }
        public static string GetHairPath(PirateCombine combine)
        {
            return $"PirateHair/{combine.hairName}";
        }
        public static string GetEaringPath(PirateCombine combine)
        {
            return $"PirateEaring/{combine.earingName}";
        }
        public static string GetBeardPath(PirateCombine combine)
        {
            return $"PirateBeard/{combine.beardName}";
        }
        public static string GetEyewearPath(PirateCombine combine)
        {
            return $"PirateEyewear/{combine.eyewearName}";
        }
        public static string GetHatPath(PirateCombine combine)
        {
            return $"PirateHat/{combine.hatName}";
        }
        public static string GetEnergyPath(PirateCombine combine)
        {
            return $"PirateEnergy/{combine.enegrySkinName}";
        }
        public static string GetProductBgPath(PirateCombine combine)
        {
            return $"PirateProductBg/{combine.productBgName}";
        }
        public static string GetDetailBgPath(PirateCombine combine)
        {
            return $"PirateDetailBg/{combine.detailBgName}";
        }
        public static string GetAuraPath(PirateCombine combine)
        {
            return $"PirateAura/{combine.auraName}";
        }
        #endregion
    }
}