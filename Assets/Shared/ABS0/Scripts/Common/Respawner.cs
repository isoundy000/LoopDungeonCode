using UnityEngine;
using System.Collections;
using UniRx;
using System;
using NodeCanvas.Framework;

public class Respawner : MonoBehaviour {

    public GameObject SummmonEffect;
    public GameObject Prefab;

    public float dueTime;
    public float period;
    public float lifeTime;
    public int maxCount;

    public float R = 2.0f;

    public string Tag;
    public string targetTag;

    // Use this for initialization
    void Start () {

        Observable.Timer(TimeSpan.FromSeconds(dueTime), TimeSpan.FromSeconds(period + 6))
            .Take(maxCount)
            .TakeWhile(t => t < lifeTime)
            .TakeUntilDestroy(gameObject)
            .Subscribe(_ =>
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * R;
            Vector3 position = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            if(SummmonEffect)
            {
                GameObject SummmonEffectObj = Instantiate(SummmonEffect, position, Quaternion.identity) as GameObject;
                Observable.Timer(TimeSpan.FromSeconds(5f))
                .TakeUntilDestroy(gameObject)
                .Subscribe(x=>
                {
                    Spawn(position);
                });
            } else
            {
                Spawn(position);
            }
            
        },()=>
        {
            Debug.Log("Complete");
        });


	}

    public void Spawn(Vector3 position)
    {
        GameObject instance = Instantiate(Prefab, position, Quaternion.identity, transform.parent) as GameObject;
        instance.tag = Tag;
        instance.layer = LayerMask.NameToLayer(Tag);

        instance.GetComponent<IBlackboard>().SetValue("TargetingTag", targetTag);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
