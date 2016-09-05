using UnityEngine;
using System.Collections;
using UniRx;

public class MagicProjectileArrow : MonoBehaviour {

	public GameObject impactParticle;
	public GameObject projectileParticle;
	public GameObject[] trailParticles;

	[HideInInspector]
	public Vector3 impactNormal; //Used to rotate impactparticle.
	public LayerMask ownerLayer;

	public float missileVelocity = 100;
	float turn = 20;
    float rato = 1.2f;
    Rigidbody homingMissile;

	public GameObject target;

	Subject<GameObject> OnHit;

	public IObservable<GameObject> OnHitAsObservable {
		get {
			return OnHit ?? (OnHit = new Subject<GameObject> ());
		}
	}

	void Start () 
	{
		projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
		projectileParticle.transform.parent = transform;
		homingMissile = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
        if(target != null)
        {
            homingMissile.velocity = transform.forward * missileVelocity;

            Vector3 targetPosition = target.transform.position;
            targetPosition.y += 1;

            var targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

            homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

            turn *= rato;
        }
    }

	void OnCollisionEnter (Collision hit) {

		if (hit.gameObject.layer == ownerLayer) {
			return;
		}

		//transform.DetachChildren();
		impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
		//Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

		//yield WaitForSeconds (0.05);
		foreach (GameObject trail in trailParticles)
		{
			Transform find = transform.Find (projectileParticle.name + "/" + trail.name);
			if (find == null) {
				return;
			}
			GameObject curTrail = find.gameObject;
			curTrail.transform.parent = null;
			Destroy(curTrail, 3f); 
		}

		OnHit.OnNext (hit.gameObject);

		Destroy(projectileParticle, 3f);
		Destroy(impactParticle, 3f);
		Destroy(gameObject);
		//projectileParticle.Stop();

	}
}
