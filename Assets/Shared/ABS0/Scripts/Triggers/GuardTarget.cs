using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;

public class GuardTarget : MonoBehaviour {

    public Action OnTargetEnter;
    public Action OnTargetExit;

    public LayerMask CheckLayer;

    List<CharacterProperty> targets;

    // Use this for initialization
    void Start () {
        targets = new List<CharacterProperty>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (CheckLayer != (CheckLayer | (1 << other.gameObject.layer)))
        {
            return;
        }

        Debug.Log(other.name + "Enter");

        CharacterProperty target = other.gameObject.GetComponent<CharacterProperty>();

        if (target != null && targets.IndexOf(target) == -1)
        {
            target.OnDiedAsObservable
                .Subscribe(t=>
                {
                    targets.Remove(t);
                });

            targets.Add(target);

            if (OnTargetEnter != null)
                OnTargetEnter.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CheckLayer != (CheckLayer | (1 << other.gameObject.layer)))
        {
            return;
        }

        Debug.Log(other.name + "Exit");

        CharacterProperty target = other.gameObject.GetComponent<CharacterProperty>();

        if (target != null)
        {
            targets.Remove(target);

            if (OnTargetExit != null)
                OnTargetExit.Invoke();
        }

    }
}
