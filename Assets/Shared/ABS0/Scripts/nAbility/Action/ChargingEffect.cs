using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class ChargingEffect : MonoBehaviour {

    public float Time = 4;
    public int AttackID = 4;

    public string TriggerName = "AttackTrigger";

    public GameObject ChargingEffectPrefab;

    public GameObject DangerIndicator;

    CharacterProperty mCharacterProperty;
    AnimatorController mAnimatorController;
    Animator mAnimator;

    // Use this for initialization
    void Start () {
        mCharacterProperty = GetComponent<CharacterProperty>();
        mAnimator = GetComponent<Animator>();
        mAnimatorController = GetComponent<AnimatorController>();

        if (mCharacterProperty != null &&
            mAnimator != null &&
            mAnimatorController != null
            )
        {
            mAnimator.SetInteger("Attack", AttackID);
            mAnimator.SetTrigger(TriggerName);
            mAnimator.SetBool("Charging", true);

            if(DangerIndicator)
            {
                DangerIndicator.gameObject.SetActive(true);
            }

            mAnimatorController.OnHitStartAsObservable
                .Take(1)
                .Subscribe(info =>
                {
                    GameObject chargingEffect = Instantiate(ChargingEffectPrefab, transform.position, transform.rotation, transform) as GameObject;

                    Observable.Timer(TimeSpan.FromSeconds(Time))
                    .TakeUntilDestroy(gameObject).Subscribe(t =>
                    {
                        Destroy(chargingEffect,2);
                        mAnimator.SetBool("Charging", false);
                        mAnimator.SetInteger("Attack", -1);
                        if (DangerIndicator)
                        {
                            DangerIndicator.gameObject.SetActive(false);
                        }
                    });
                });
        }
        else
        {
            Destroy(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
