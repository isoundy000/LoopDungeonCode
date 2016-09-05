using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class TriggerStayTrigger : MonoBehaviour, ITrigger {

    public LayerMask CheckLayer;

    Subject<CharacterProperty> OnBeTriggerred;

    public IObservable<CharacterProperty> OnBeTriggerredObservable()
    {
        return OnBeTriggerred ?? (OnBeTriggerred = new Subject<CharacterProperty>());
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other) {
        if (CheckLayer != (CheckLayer | (1 << other.gameObject.layer)))
        {
            return;
        }

        CharacterProperty target = other.gameObject.GetComponent<CharacterProperty>();

        if (target != null && OnBeTriggerred != null)
        {
            OnBeTriggerred.OnNext(target);
        }
    }
}
