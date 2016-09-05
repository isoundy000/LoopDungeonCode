using UnityEngine;
using System.Collections;
using UniRx;
using System;
using NodeCanvas.Framework;

public class SummonAbility : ActionAbility
{

    CharacterProperty mOwner;

    public bool Check = true;


    public GameObject SummmonEffect;
    public GameObject Prefab;

    public int Count = 1;
    public float R = 2.0f;

    public string Tag;
    public string targetTag;

    // Use this for initialization
    void Start()
    {
        mOwner = GetComponent<CharacterProperty>();

        InitTriggerObserver(target =>
        {
            if (Check)
            {
                Cast(mOwner, target);
            }
            else
            {
                Cast(target);
            }

        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void DoAbility(CharacterProperty target)
    {
        for (int i = 0; i < Count; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * R;
            Vector3 position = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (SummmonEffect)
            {
                GameObject SummmonEffectObj = Instantiate(SummmonEffect, position, Quaternion.identity) as GameObject;
                Observable.Timer(TimeSpan.FromSeconds(5f))
                .TakeUntilDestroy(gameObject)
                .Subscribe(x =>
                {
                    Spawn(position);
                });
            }
            else
            {
                Spawn(position);
            }
        }
    }

    public void Spawn(Vector3 position)
    {
        position.y = 2;
        GameObject instance = Instantiate(Prefab, position, Quaternion.identity, transform.parent) as GameObject;
        instance.tag = Tag;
        instance.layer = LayerMask.NameToLayer(Tag);

        instance.GetComponent<IBlackboard>().SetValue("TargetingTag", targetTag);
    }
}
