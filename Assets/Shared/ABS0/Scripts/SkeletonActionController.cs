using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using NodeCanvas.BehaviourTrees;
using ABS0;

public class SkeletonActionController : MonoBehaviour
{
    public GameObject DeathEffect;

    static int mWalkRunState = Animator.StringToHash("Base Layer.Movement.WalkRunBlend");
    static int mAttackState = Animator.StringToHash("Base Layer.Attacks.Attack");
    static int mKnockbackState = Animator.StringToHash("Base Layer.Reactions.Knockback");

    static int mDeathState = Animator.StringToHash("Base Layer.Death.Death");

    Animator mAnimator;
    CharacterMotor mCharacterController;
    ObservableStateMachineTrigger mObservableStateMachineTrigger;
    BehaviourTreeOwner mBehaviourTreeOwner;

    public GameObject ChargingEffectPrefab;

    public GameObject RectDangerArea;
    public GameObject CircleDangerArea;

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
    }

    void OnEnterState(AnimatorStateInfo info)
    {
        if(info.fullPathHash == mKnockbackState)
        {
            mAnimator.ResetTrigger("AttackTrigger");
        }
        else if (info.fullPathHash == mDeathState)
        {
            mBehaviourTreeOwner.StopBehaviour();
            Observable.Timer(TimeSpan.FromSeconds(3.0f)).Subscribe(_ =>
            {
                GameObject DeathEffectObj = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
                Destroy(DeathEffectObj, 1.0f);
                Destroy(gameObject);
            });

        }

        if(info.fullPathHash == mAttackState ||
            info.fullPathHash == mDeathState ||
            info.fullPathHash == mKnockbackState)
        {
            mCharacterController.SetMoveable(false);
        } else
        {
            mCharacterController.SetMoveable(true);
        }
    }

    void OnExitState(AnimatorStateInfo info)
    {

    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

        if(animatorStateInfo.fullPathHash == mKnockbackState)
        {
            transform.position = -transform.forward * Time.deltaTime * 0.3f + transform.position;
        }
    }

    void LateUpdate()
    {
        mAnimator.SetFloat("Speed", mCharacterController.GetVelocity().magnitude);
    }

    public void Attack(int id)
    {
        mAnimator.SetInteger("Attack", id);
        mAnimator.SetTrigger("AttackTrigger");
    }

    public void ChargeAttack(int id, float time)
    {
        ChargingEffect action = gameObject.AddComponent<ChargingEffect>();
        action.ChargingEffectPrefab = ChargingEffectPrefab;
        action.Time = time;
        action.AttackID = id;
        action.DangerIndicator = RectDangerArea;
    }
}
