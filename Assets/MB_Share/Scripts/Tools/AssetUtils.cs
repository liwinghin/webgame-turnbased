using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetUtils
{
    #region Project Resource
    public static IEnumerator LoadAsset<T>(string address, Action<T> callBack) where T : UnityEngine.Object
    {
        var loader = Addressables.LoadAssetAsync<T>(address);
        yield return loader;

        T result = null;
        if (loader.Status == AsyncOperationStatus.Succeeded)
        {
            result = loader.Result;
        }

        callBack.Invoke(result);
    }

    public static IEnumerator LoadPrefab<T>(string address, Action<T> callBack) where T : Component
    {
        var loader = Addressables.LoadAssetAsync<GameObject>(address);
        yield return loader;

        T result = null;
        if (loader.Status == AsyncOperationStatus.Succeeded)
        {
            result = loader.Result.GetComponent<T>();
        }

        callBack.Invoke(result);
    }
    #endregion

    #region External resource
    public static IEnumerator LoadTextFile(string url, bool localFile, Action<string> callBack)
    {
        string content = string.Empty;

        if (localFile)
            url = "file://" + url;

        // HTTP load from path
        UnityWebRequest www = UnityWebRequest.Get(url);
        using (www)
        {
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                content = www.downloadHandler.text;
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        // call back
        callBack.Invoke(content);
    }

    public static IEnumerator LoadTexture(string url, bool localFile, Action<Texture2D> callBack)
    {
        Texture2D content = null;

        if (localFile)
            url = "file://" + url;

        // HTTP load from path
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        using(www)
        {
            yield return www.SendWebRequest();

            if (string.IsNullOrEmpty(www.error))
            {
                content = DownloadHandlerTexture.GetContent(www);
            }
            else
            {
                Debug.Log(www.error);
            }
        }

        // call back
        callBack.Invoke(content);
    }

    public static IEnumerator SaveTextFile(string directory, string fileName, string content, Action callBack)
    {
        string filePath = Path.Combine(directory, fileName);

        // Create directory
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Write to path
        File.WriteAllText(filePath, content);
        yield return new WaitForSeconds(0.5f);

        // call back
        callBack.Invoke();
    }

    public static void DeleteDirectory(string directory)
    {
        if (!Directory.Exists(directory)) return;

        foreach (string subdirectory in Directory.GetDirectories(directory))
        {
            Directory.Delete(subdirectory);
        }

        try
        {
            Directory.Delete(directory, true);
        }
        catch (IOException)
        {
            Directory.Delete(directory, true);
        }
        catch (UnauthorizedAccessException)
        {
            Directory.Delete(directory, true);
        }
    }
    #endregion

    #region Data Model
    public static IEnumerator LoadData<T>(string filePath, bool localFile, Action<T> callBack)
    {
        T data = default(T);
        yield return LoadTextFile(filePath, localFile, (json) => {
            if (!string.IsNullOrEmpty(json))
            {
                data = JsonUtility.FromJson<T>(json);
            }
        });
        callBack.Invoke(data);
    }

    public static IEnumerator SaveData<T>(string directory, string fileName, T data, Action callBack)
    {
        var json = JsonUtility.ToJson(data, true);
        yield return SaveTextFile(directory, fileName, json, callBack);
    }
    #endregion
}