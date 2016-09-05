using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class AfterDiedEffect : Effect
{

    GameObject effectInstance;
    string mName;

    public AfterDiedEffect(string name) : base(float.PositiveInfinity)
    {
        mName = name;
    }

    protected override void OnActive(CharacterProperty target)
    {
        target.OnDiedAsObservable.Subscribe(_ =>
           {
               GameObject Prefab = Resources.Load<GameObject>(mName);
               effectInstance = UnityEngine.Object.Instantiate(Prefab, target.transform.position, target.transform.rotation) as GameObject;
               UnityEngine.Object.Destroy(effectInstance, 2.0f);
               UnityEngine.Object.Destroy(target.gameObject);
           });
    }

    protected override void OnTimeout(CharacterProperty target)
    {

    }

    protected override void OnCanceled(CharacterProperty target)
    {

    }

    public override object Clone()
    {
        AfterDiedEffect instance = (AfterDiedEffect)base.Clone();
        return instance;
    }
}
