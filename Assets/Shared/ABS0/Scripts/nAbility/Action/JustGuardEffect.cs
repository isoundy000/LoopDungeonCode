using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class JustGuardEffect : MonoBehaviour {

    public float Time = 30;

    public string TriggerName = "Block";
    public int AttackID = 3;

    CharacterProperty mCharacterProperty;
    AnimatorController mAnimatorController;
    Animator mAnimator;
    bool InBlock;

    // Use this for initialization
    void Start()
    {
        mCharacterProperty = GetComponent<CharacterProperty>();
        mAnimator = GetComponent<Animator>();
        mAnimatorController = GetComponent<AnimatorController>();

        if (mCharacterProperty != null &&
            mAnimator != null &&
            mAnimatorController != null
            )
        {
            mAnimator.SetBool("CounterAttackResult", false);

            float time = UnityEngine.Time.time;

            mCharacterProperty.BeforeDamageTakeAsObservable
            .Take(1)
            .TakeUntilDestroy(this)
            .Timeout(TimeSpan.FromSeconds(Time))
            .Subscribe(damage =>
            {
                float deltaTime = UnityEngine.Time.time - time;
                Debug.Log("block to hit time->" + deltaTime.ToString());
                damage.value = 0;

                if(deltaTime < 0.3f && deltaTime > 0.1f)
                {
                    mAnimator.SetBool("CounterAttackResult", true);
                    GameObject prefab = Resources.Load("Effects/LightCharge") as GameObject;
                    GameObject effect = Instantiate(prefab, transform.TransformPoint(prefab.transform.position), Quaternion.identity, transform) as GameObject;
                    Destroy(effect, 0.5f);
                    mAnimator.SetInteger("Attack", 3);
                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => {
                        Destroy(effect);
                        mAnimator.SetInteger("Attack", 0);
                        mAnimator.SetBool("CounterAttackResult", false);
                    });
                } else
                {
                    mAnimator.SetBool("CounterAttackResult", false);
                    Debug.Log("Block too early or late");
                }
                Destroy(this);
            },
            ex => {
                mAnimator.SetBool("CounterAttackResult", false);
                Destroy(this);
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
