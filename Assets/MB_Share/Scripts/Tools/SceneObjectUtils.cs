using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectUtils
{
    public static void SetParent(Transform a, Transform b)
    {
        Vector3 localScale = a.localScale;
        a.SetParent(b);
        a.localPosition = Vector3.zero;
        a.localRotation = Quaternion.identity;
        a.localScale = localScale;
    }

    public static void DestroyChildren(Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; --i)
        {
            var child = transform.GetChild(i);
            Object.Destroy(child.gameObject);
        }
    }

    public static void FollowPosition(Transform a, Transform b)
    {
        a.position = b.position;
    }

    public static void FollowRotation(Transform a, Transform b)
    {
        a.rotation = b.rotation;
    }
    public static void FollowRotation(Transform a, Transform b, Vector3 offset)
    {
        a.rotation = b.rotation * Quaternion.Euler(offset);
    }

    public static void SetActiveMaterial(Material material, params Renderer[] renderers)
    {
        foreach (var renderer in renderers)
        {
            renderer.material = material;
            renderer.SetActive(material != null);
        }
    }
}
