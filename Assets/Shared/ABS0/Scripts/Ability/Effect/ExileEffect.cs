using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ExileEffect : Effect
{
    GameObject effectInstance;
    public float mSlowRatio;

    public ExileEffect(float duration) : base(duration)
    {
        
    }

    protected override void OnActive(CharacterProperty target)
    {

        GameObject Prefab = Resources.Load<GameObject>("Effects/States/SCFX_Exile");
        effectInstance = GameObject.Instantiate(Prefab, target.transform.position, target.transform.rotation) as GameObject;
        effectInstance.transform.SetParent(target.transform);
        effectInstance.transform.localPosition = Prefab.transform.position;


    }

    protected override void OnTimeout(CharacterProperty target)
    {
        target.ResetWalkSpeed();
        GameObject.Destroy(effectInstance);
    }

    public override object Clone()
    {
        SlowEffect instance = (SlowEffect)base.Clone();
        return instance;
    }
}
