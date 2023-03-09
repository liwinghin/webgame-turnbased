using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CamBackground : MonoBehaviour, ICamPart
    {
        public string partName => this.name;

        #region Asset
        public static IEnumerator Load(string path, Action<CamBackground> callBack)
        {
            yield return AssetUtils.LoadPrefab(path, callBack);
        }
        #endregion

        #region Part hanle
        public void StartAttach(CamRuntime camView)
        {
            // Add and Attach
            camView.nowBackground = this;
            this.Attach(camView.transform);
        }

        public void StopAttach(CamRuntime camView)
        {
            // Remove and Destroy
            camView.nowBackground = null;
            Destroy(this.gameObject);
        }
        #endregion
    }
}