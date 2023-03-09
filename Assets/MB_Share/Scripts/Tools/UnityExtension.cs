using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public static class UnityExtension
{
    public static void SetActive(this Component component, bool activeValue)
    {
        component.gameObject.SetActive(activeValue);
    }

    public static T Clone<T>(this T uObj) where T : Object
    {
        var clone = Object.Instantiate<T>(uObj);
        clone.name = uObj.name;
        return clone;
    }

    public static void ForceRebuild(this LayoutGroup layoutGroup)
    {
        var rectTrans = (RectTransform)layoutGroup.transform;
        layoutGroup.StartCoroutine(RebuildThread(rectTrans));
    }

    private static IEnumerator RebuildThread(RectTransform layoutRect)
    {
        yield return new WaitForEndOfFrame();

        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRect);
    }
}