using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

[Serializable]
public class CodeContain
{
    public List<string> codes = new List<string>();

    #region Asset
    public IEnumerator Load(string directory, string fileName)
    {
        var path = Path.Combine(directory, fileName);
        yield return AssetUtils.LoadData<CodeContain>(path, true, (data) => {
            if (data != null)
            {
                this.codes = data.codes;
            }
        });
    }

    public IEnumerator Save(string directory, string fileName)
    {
        yield return AssetUtils.SaveData<CodeContain>(directory, fileName, this, () => { });
    }
    #endregion
}