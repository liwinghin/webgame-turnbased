using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneScript01 : MonoBehaviour {

    public List<Material> effects = new List<Material>();
    public SkinnedShield skinnedShield;
    public Text descText;

    int currentEffect = 0;

    void Start()
    {
        currentEffect = 0;
        skinnedShield.SetMaterial(effects[currentEffect]);
        descText.text = "Shield " + (currentEffect + 1);
    }

    public void Next()
    {
        currentEffect++;
        currentEffect %= effects.Count;
        skinnedShield.SetMaterial(effects[currentEffect]);
        descText.text = "Shield " + (currentEffect + 1);
    }

    public void Prev()
    {
        currentEffect = (currentEffect == 0) ? effects.Count - 1 : currentEffect - 1;
        skinnedShield.SetMaterial(effects[currentEffect]);
        descText.text = "Shield " + (currentEffect + 1);
    }

    public void ToogleOnOff()
    {
        if (skinnedShield.effectOn)
            skinnedShield.SetEffectOff();
        else
            skinnedShield.SetEffectOn();
    }
}
