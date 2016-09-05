using UnityEngine;
using System.Collections;
using System;
using UniRx;

public class TimedTrigger : MonoBehaviour {

    public CharacterProperty Reference;
    public int RepeatCount;
    public float Time = 1.0f;

    int count;
    Subject<CharacterProperty> OnBeTriggerred;

    public IObservable<CharacterProperty> OnBeTriggerredObservable()
    {
        return OnBeTriggerred ?? (OnBeTriggerred = new Subject<CharacterProperty>());
    }

    // Use this for initialization
    void Start () {
        count = 0;
        Observable.Interval(TimeSpan.FromSeconds(Time))
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
        {
            count++;
            if(OnBeTriggerred != null)
            {
                OnBeTriggerred.OnNext(Reference);
            }
           
            if (RepeatCount <= count)
            {
                if (OnBeTriggerred != null)
                {
                    OnBeTriggerred.OnCompleted();
                }
                
                Destroy(this);
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
