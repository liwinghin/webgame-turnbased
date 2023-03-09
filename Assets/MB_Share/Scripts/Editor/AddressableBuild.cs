using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace MB
{
    public class ContentBuilder
    {
        private const string PirateGroupName = "MB_Pirate";
        private const string ShipGroupName = "MB_Ship";
        private const string CombatGroupName = "MB_Combat";

        [MenuItem("Tools/Build Content/Only Pirate")]
        public static void BuildPirate()
        {
            BuildContent(PirateGroupName);
        }

        [MenuItem("Tools/Build Content/Only Ship")]
        public static void BuildShip()
        {
            BuildContent(ShipGroupName);
        }

        [MenuItem("Tools/Build Content/For Combat")]
        public static void BuildCombat()
        {
            BuildContent(PirateGroupName, ShipGroupName, CombatGroupName);
        }

        private static void BuildContent(params string[] groupNames)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var nameList = new List<string>(groupNames);

            foreach (var sg in settings.groups)
            {
                if (sg.ReadOnly) continue;

                var groupSchema = sg.GetSchema<BundledAssetGroupSchema>();
                groupSchema.IncludeInBuild = nameList.Contains(sg.Name);
                EditorUtility.SetDirty(sg);
            }

            AddressableAssetSettings.BuildPlayerContent();
        }
    }
}
#endif