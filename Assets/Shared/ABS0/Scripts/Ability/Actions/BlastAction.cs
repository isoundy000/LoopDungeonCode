using UnityEngine;
using System.Collections;

public class BlastAction : AbilityAction {

	Animator mAnimator;

	public GameObject BlastSkillPrefab;

	GameObject instance;

	float mRadius;
	string mName;
	int mMaxTargetCount;

	public BlastAction (float radius, int maxTargetCount, string name){
		mRadius = radius;
		mName = name;
		mMaxTargetCount = maxTargetCount;
	}

    protected override void Start()
    {
        GameObject Prefab = Resources.Load<GameObject>(mName);
        GameObject effectInstance = GameObject.Instantiate(Prefab, mOwner.transform.position, mOwner.transform.rotation) as GameObject;
        effectInstance.transform.SetParent(mOwner.transform);
        GameObject.Destroy(effectInstance, 2.0f);
        //Collider[] colliders = Physics.OverlapSphere(target.transform.position, mRadius, target.OppositeLayer);
        status = AbilityActionStatus.Success;
    }
}
