using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;

namespace MB
{
    public class AssetUpdateWindow : EditorWindow
    {
        private const string WindowName = "Assets Update";
        private const string NumberKey = "{number}";
        private const string FileNameKey = "{fileName}";

        private string m_folderPath = string.Empty;
        private string m_numNameFormat = string.Empty;
        private int m_numNameStart = 0;
        private int m_numNamePad = 1;
        private string m_addressPathFormat = string.Empty;

        [MenuItem("Tools/Window/" + WindowName)]
        private static void OpenWindow()
        {
            var window = GetWindow<AssetUpdateWindow>(true, WindowName, true);
            window.m_numNameFormat = NumberKey;
            window.m_addressPathFormat = FileNameKey;
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Target Assets", EditorStyles.boldLabel);
            this.m_folderPath = EditorGUILayout.TextField("Folder Path", this.m_folderPath);

            this.FolderPathGui();
            this.NumberNameGui();
            this.AddressablePathGui();
        }

        #region Gui
        private void FolderPathGui()
        {
            if (GUILayout.Button("Browse", GUILayout.Width(100)))
            {
                var path = EditorUtility.OpenFolderPanel("Folder", "", "");
                if (!string.IsNullOrEmpty(path))
                {
                    path = path.Replace(Application.dataPath+"/", "");
                    this.m_folderPath = path;
                }
            }
        }

        private void NumberNameGui()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Number Name", EditorStyles.boldLabel);
            this.m_numNameStart = EditorGUILayout.IntField("Number Name Start", this.m_numNameStart);
            this.m_numNamePad = EditorGUILayout.IntField("Number Name Pad", this.m_numNamePad);
            this.m_numNameFormat = EditorGUILayout.TextField("Number Name Format", this.m_numNameFormat);
            EditorGUILayout.LabelField($"{NumberKey} means file number.", EditorStyles.miniLabel);

            if (GUILayout.Button("Apply"))
            {
                this.ApplyNumberName();
            }
        }

        private void AddressablePathGui()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Addressable Path", EditorStyles.boldLabel);
            this.m_addressPathFormat = EditorGUILayout.TextField("Addressable Path Format", this.m_addressPathFormat);
            EditorGUILayout.LabelField($"{FileNameKey} means file name.", EditorStyles.miniLabel);
            if (GUILayout.Button("Apply"))
            {
                this.ApplyAddressablePath();
            }
        }
        #endregion

        #region Asset handle
        private void ApplyNumberName()
        {
            var guids = AssetDatabase.FindAssets("*", new string[] { $"Assets/{this.m_folderPath}" });
            var fmt = new string('0', this.m_numNamePad);
            var num = 0;

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var offNum = this.m_numNameStart + i;
                var padNum = offNum.ToString(fmt);
                AssetDatabase.RenameAsset(path, this.m_numNameFormat.Replace(NumberKey, padNum));
                num++;
            }

            Debug.Log($"Apply number total {num}.");
        }

        private void ApplyAddressablePath()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var guids = AssetDatabase.FindAssets("*", new string[] { $"Assets/{this.m_folderPath}" });
            var num = 0;

            foreach (var guid in guids)
            {
                var entry = settings.FindAssetEntry(guid);
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var filename = Path.GetFileNameWithoutExtension(path);
                entry.address = this.m_addressPathFormat.Replace(FileNameKey, filename);
                num++;
            }

            Debug.Log($"Apply addressable total {num}.");
        }
        #endregion
    }
}
#endif