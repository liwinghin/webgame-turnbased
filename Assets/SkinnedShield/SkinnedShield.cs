using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedShield : MonoBehaviour {
    
    public Material newMaterial;
    public bool effectOn = true;

    private Transform rootTransform;
    List<GameObject> result=new List<GameObject>();

    private void Start()
    {
        rootTransform = transform;
        AddSkinnedMeshTo(gameObject, rootTransform);        
    }

    public void SetEffectOn()
    {
        effectOn = true;
        foreach (GameObject go in result)
            go.SetActive(true);
    }

    public void SetEffectOff()
    {
        effectOn = false;
        foreach (GameObject go in result)
            go.SetActive(false);
    }

    public void SetMaterial(Material mat)
    {
        newMaterial = mat;
        foreach (GameObject go in result)
        {
            SkinnedMeshRenderer NewRenderer = go.GetComponent<SkinnedMeshRenderer>();
            Material[] materials = { newMaterial };
            NewRenderer.materials = materials;
        }
    }

    public void AddSkinnedMeshTo(GameObject obj, Transform root)
    {
        SkinnedMeshRenderer[] BonedObjects = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in BonedObjects)
        {
            result.Add(ProcessBonedObject(smr, root));
        }
    }

    private GameObject ProcessBonedObject(SkinnedMeshRenderer ThisRenderer, Transform root)
    {
        GameObject newObject = new GameObject(ThisRenderer.gameObject.name);
        newObject.transform.parent = root;
        SkinnedMeshRenderer NewRenderer = newObject.AddComponent(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;
        Transform[] MyBones = new Transform[ThisRenderer.bones.Length];
        for (int i = 0; i < ThisRenderer.bones.Length; i++)
            MyBones[i] = FindChildByName(ThisRenderer.bones[i].name, root);	
        NewRenderer.bones = MyBones;
        NewRenderer.sharedMesh = ThisRenderer.sharedMesh;
        Material[] materials = { newMaterial };
        NewRenderer.materials = materials;
        NewRenderer.rootBone = ThisRenderer.rootBone;
        return newObject;
    }

    private  Transform FindChildByName(string ThisName, Transform ThisGObj)
    {
        Transform ReturnObj;
        if (ThisGObj.name == ThisName)
            return ThisGObj.transform;
        foreach (Transform child in ThisGObj)
        {
            ReturnObj = FindChildByName(ThisName, child);
            if (ReturnObj != null)
                return ReturnObj;
        }
        return null;
    }
}
