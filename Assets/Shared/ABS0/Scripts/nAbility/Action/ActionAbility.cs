using UnityEngine;
using System.Collections;
using UniRx;

public class ActionAbility : MonoBehaviour
{
    public GameObject TriggerObject;

    ITrigger mTriggerTarget;

    public float CoolDown;
    public float Cost;
    public float MaxRange = float.PositiveInfinity;

    float mLastTime = float.NegativeInfinity;


    public float CoodDownLeft
    {
        get
        {
            return CoolDown - (Time.time - mLastTime);
        }
    }

    public bool CanCast(CharacterProperty owner, CharacterProperty target)
    {
        if (!owner.HasEnoughMP(Cost) || (CoodDownLeft > 0))
        {
            return false;
        }

        Transform ProjectileCastPoint = owner.transform.FindChild("ProjectileCastPoint");
        Transform CastPoint = (ProjectileCastPoint == null) ? owner.transform : ProjectileCastPoint;

        float distance = Vector3.Distance(CastPoint.position, target.transform.position);
        if (distance > MaxRange)
        {
            return false;
        }

        return true;
    }

    protected void InitTriggerObserver(System.Action<CharacterProperty> action)
    {
        mTriggerTarget = TriggerObject.GetComponent<ITrigger>();
        mTriggerTarget.OnBeTriggerredObservable()
            .Subscribe(action);
    }

    public void Cast(CharacterProperty owner, CharacterProperty target)
    {
        if (CanCast(owner, target))
        {
            DoAbility(target);
            mLastTime = Time.time;
        }
    }

    public void Cast(CharacterProperty target)
    {
        if((CoodDownLeft <= 0))
        {
            DoAbility(target);
            mLastTime = Time.time;
        }

    }

    protected virtual void DoAbility(CharacterProperty target)
    {

    }
}