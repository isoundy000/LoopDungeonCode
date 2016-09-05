using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class TakaraBakoController : MonoBehaviour, ITrigger {

    public GameObject DeathEffect;

    CharacterProperty mCharacterProperty;
    Animator mAnimator;

    Subject<CharacterProperty> OnBeTriggerred;

    public IObservable<CharacterProperty> OnBeTriggerredObservable()
    {
        return OnBeTriggerred ?? (OnBeTriggerred = new Subject<CharacterProperty>());
    }

    // Use this for initialization
    void Start () {
        mCharacterProperty = GetComponent<CharacterProperty>();
        mAnimator = GetComponent<Animator>();

        mCharacterProperty
            .OnDiedAsObservable
            .Subscribe(t =>
            {
                mAnimator.SetBool("Open", true);

                if(OnBeTriggerred != null)
                {
                    OnBeTriggerred.OnNext(t);
                    OnBeTriggerred.OnCompleted();
                }

                Observable.Timer(TimeSpan.FromSeconds(3.0f))
                .TakeUntilDestroy(this)
                .Subscribe(_=>
                {
                    GameObject DeathEffectObj = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
                    Destroy(DeathEffectObj, 1.0f);
                    Destroy(gameObject);
                });
            });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
