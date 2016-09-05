using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class TeleportTo : MonoBehaviour {

    public GameObject TeleportEffect;
    public GameObject to;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Teleport(Action completed)
    {
        GameObject SummmonEffectObj = Instantiate(TeleportEffect, transform.position, Quaternion.identity) as GameObject;
        Observable.Timer(TimeSpan.FromSeconds(5f))
        .TakeUntilDestroy(gameObject)
        .Subscribe(x =>
        {
            transform.position = to.transform.position;
            completed();
        });
    }
}
