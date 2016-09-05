using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

    public CharacterProperty mOwner;

    float t;

    public Collider CheckCollider;

    private AttackActionInfo mAttackActionInfo;

    public AttackActionInfo AttackActionInfo
    {
        set
        {
            if (mAttackActionInfo == value)
            {
                return;
            }

            mAttackActionInfo = value;

            if (mAttackActionInfo != null)
            {
                t = Time.time;
            }

            CheckCollider.enabled = (value != null);
        }
    }

    // Use this for initialization
    void Start () {
        //mOwner = GetComponentInParent<CharacterProperty>();

        CheckCollider.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(mAttackActionInfo == null)
        {
            return;
        }

        if(other.gameObject == mOwner.gameObject)
        {
            return;
        }

        HitCollider hitObj = other.gameObject.GetComponent<HitCollider>();

        if(hitObj == null)
        {
            return;
        }

        CharacterProperty character = hitObj.mOwner;

        if (character)
        {
            float hitTime = Time.time - t;

            character.Hit(mOwner, mAttackActionInfo.value);
            //character.HitRate
            GameObject hitEffect = Instantiate(Resources.Load("Effects/Hits/SimpleHitFlash"), CheckCollider.ClosestPointOnBounds(other.transform.position), Quaternion.identity) as GameObject;
            Destroy(hitEffect, 3.0f);

            mAttackActionInfo = null;
            CheckCollider.enabled = false;
        }
    }
}
