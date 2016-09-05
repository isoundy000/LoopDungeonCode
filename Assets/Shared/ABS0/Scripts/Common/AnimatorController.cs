using UnityEngine;
using System.Collections;
using NodeCanvas.Framework;
using UniRx;

public class AnimatorController : MonoBehaviour {

    //protected Subject<AttackActionInfo> OnAttack;

    //public IObservable<AttackActionInfo> OnAttackAsObservable
    //{
    //    get
    //    {
    //        return OnAttack ?? (OnAttack = new Subject<AttackActionInfo>());
    //    }
    //}

    Subject<AttackActionInfo> OnHitStart;

    public IObservable<AttackActionInfo> OnHitStartAsObservable
    {
        get
        {
            return OnHitStart ?? (OnHitStart = new Subject<AttackActionInfo>());
        }
    }

    Subject<AttackActionInfo> OnHitEnd;

    public IObservable<AttackActionInfo> OnHitEndAsObservable
    {
        get
        {
            return OnHitEnd ?? (OnHitEnd = new Subject<AttackActionInfo>());
        }
    }

    CharacterProperty mCharacterProperty;
    Animator mAnimator;
    Rigidbody mRigidbody;

    // Use this for initialization
    void Start () {
        mCharacterProperty = GetComponent<CharacterProperty>();
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();


        if (mCharacterProperty != null &&
            mAnimator != null
            )
        {
            mCharacterProperty.OnDamageTakeAsObservable
                .TakeUntilDestroy(this)
                .Subscribe(damage =>
            {
                if (damage.ignore)
                    return;

                if (damage.power >= 10)
                {
                    if(damage.from != null)
                    {
                        transform.LookAt(damage.from.transform);
                    }
                    mAnimator.SetTrigger("HeavyHitTrigger");
                }
                else if( damage.power >= 5 && damage.power < 10)
                {
                    mAnimator.SetTrigger("LightHitTrigger");
                }
            });

            mCharacterProperty.OnDiedAsObservable
                .TakeUntilDestroy(this)
                .Subscribe(_ =>
            {
                mAnimator.SetBool("Died", true);
                mAnimator.SetTrigger("DeathTrigger");
            });
        }

    }

    #region Updates
    // Update is called once per frame
    void Update () {
	
	}

    #endregion

    void HitStart(AttackActionInfo info)
    {
        if (OnHitStart != null)
            OnHitStart.OnNext(info);
    }

    void HitEnd(AttackActionInfo info)
    {
        if (OnHitEnd != null)
            OnHitEnd.OnNext(info);
    }

    void ChainHint(int attackID)
    {
        mAnimator.SetInteger("Attack", attackID);
        Debug.Log("next chain" + attackID);
    }
}
