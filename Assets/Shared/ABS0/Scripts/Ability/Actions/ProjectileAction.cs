using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class ProjectileAction : AbilityAction {
	static readonly string Prefix = "Effects/Projectiles/";
	GameObject Prefab;
	public string mName;
	public float mSpeed;

	float mStartTime;

	public ProjectileAction(float maxRange, string name, float speed) {
		mSpeed = speed;
		mName = name;
		MaxRange = maxRange;
		Prefab = Resources.Load<GameObject> (Prefix + mName);
	}

	protected override void Start ()
	{
		Transform ProjectileCastPoint = mOwner.transform.Find ("ProjectileCastPoint");
		Transform CastPoint = (ProjectileCastPoint == null) ? mOwner.transform : ProjectileCastPoint;

        foreach (CharacterProperty target in mTargets) {
            if (target == null)
            {
                continue;
            }
            Vector3 targetPoint = target.transform.position;
            targetPoint.y += 2;
            GameObject projectile = GameObject.Instantiate(Prefab, CastPoint.position, CastPoint.rotation) as GameObject;
            //projectile.transform.LookAt (targetPoint);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * mSpeed);
            MagicProjectileArrow arrow = projectile.GetComponent<MagicProjectileArrow>();
            arrow.impactNormal = CastPoint.position - targetPoint;
            arrow.gameObject.layer = LayerMask.NameToLayer("Effect" + LayerMask.LayerToName(mOwner.gameObject.layer));
            arrow.ownerLayer = mOwner.gameObject.layer;
            arrow.target = target.gameObject;
            arrow.missileVelocity = mSpeed;

            arrow.OnHitAsObservable.Timeout(TimeSpan.FromSeconds(10)).Subscribe(_ =>
            {
                status = AbilityActionStatus.Success;
            }, ex =>
            {
                UnityEngine.Object.Destroy(projectile);
                status = AbilityActionStatus.Failure;
            });
            

		}

		mStartTime = Time.time;
		status = AbilityActionStatus.Running;

	}

	protected override void Update ()
	{
		if (status != AbilityActionStatus.Success) {
			status = AbilityActionStatus.Running;
		}
	}
}
