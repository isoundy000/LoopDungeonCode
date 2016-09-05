using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using NodeCanvas.BehaviourTrees;
using ABS0;

public class SDHeroActionController : MonoBehaviour {

    public GameObject BirthEffect;
    public GameObject DeathEffect;

    static int mIdleState = Animator.StringToHash("Base Layer.Idle");
    static int mWalkRunState = Animator.StringToHash("Base Layer.Movement.Locomotion");
    //static int mAttackState = Animator.StringToHash("Base Layer.Attacks.Attack");
    static int mKnockbackState = Animator.StringToHash("Base Layer.Reactions.Knockback");

    static int mDeathDie1State = Animator.StringToHash("Base Layer.Death.die1");
    static int mDeathDie2State = Animator.StringToHash("Base Layer.Death.die2");
    static int mDeathDie3State = Animator.StringToHash("Base Layer.Death.die3");

    Animator mAnimator;
    CharacterMotor mCharacterController;
    ObservableStateMachineTrigger mObservableStateMachineTrigger;

    BehaviourTreeOwner mBehaviourTreeOwner;

    bool mMoveable = true;

    public bool Moveable
    {
        get
        {
            return mMoveable;
        }

        set
        {
            mMoveable = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mCharacterController = GetComponent<CharacterMotor>();
        mBehaviourTreeOwner = GetComponent<BehaviourTreeOwner>();

        mObservableStateMachineTrigger = mAnimator.GetBehaviour<ObservableStateMachineTrigger>();

        mObservableStateMachineTrigger
            .OnStateExitAsObservable()
            .Subscribe(info =>
            {
                OnExitState(info.StateInfo);
            });

        mObservableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Subscribe(info =>
            {
                OnEnterState(info.StateInfo);
            });

        mAnimator.SetInteger("Death", Mathf.RoundToInt(UnityEngine.Random.Range(1, 3)));
    }

    void OnEnterState(AnimatorStateInfo info)
    {
        if (info.fullPathHash == mKnockbackState)
        {
            mAnimator.ResetTrigger("AttackTrigger");
        }
        else if(info.fullPathHash == mDeathDie1State ||
            info.fullPathHash == mDeathDie2State ||
            info.fullPathHash == mDeathDie3State)
        {
            mBehaviourTreeOwner.StopBehaviour();
            Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
            {
                GameObject DeathEffectObj = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(DeathEffectObj, 1.0f);
                Destroy(gameObject);
            });
            
        }
    }

    void OnExitState(AnimatorStateInfo info)
    {

    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.fullPathHash == mKnockbackState)
        {
            transform.position = -transform.forward * Time.deltaTime * 0.3f + transform.position;
        }
    }

    void LateUpdate()
    {
        float speed = mCharacterController.GetVelocity().magnitude;
        mAnimator.SetFloat("Speed", speed);
    }

    public void Attack(int id)
    {
        mAnimator.SetInteger("Attack", id);
        mAnimator.SetTrigger("AttackTrigger");
    }
}
