using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ForceEffect : Effect {

	public ForceEffect() : base(0.0f) {
		
	}

//	public float Damage;
//
//	public Vector3 forcePosition;
//
//	Rigidbody mRigidbody;
//
//	// Use this for initialization
//	void Start () {
//		mRigidbody = GetComponent<Rigidbody> ();
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//
//	public override void Execute ()
//	{
//		if (mRigidbody == null) {
//			mRigidbody = GetComponent<Rigidbody> ();
//		}
//		mRigidbody.AddExplosionForce (100.0f, forcePosition, 1);
//	}
}
