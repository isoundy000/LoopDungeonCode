using UnityEngine;
using System.Collections;
using UniRx;
public class TriggerListener : MonoBehaviour {

    public GameObject TriggerObject;
    ITrigger mTriggerTarget;

    public MonoBehaviour ToEnable;

	// Use this for initialization
	void Start () {
        mTriggerTarget = TriggerObject.GetComponent<ITrigger>();

        mTriggerTarget.OnBeTriggerredObservable()
            .Subscribe(
                _ =>
                {
                    ToEnable.enabled = true;
                }
            );
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
