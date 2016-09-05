using UnityEngine;
using System.Collections;
using UniRx;

public class HomingTrigger : MonoBehaviour, ITrigger
{

    public float moveSpeed;
    CharacterProperty target;
    public float TriggerDistance;
    public float Delay;
    public LayerMask CheckLayer;

    Subject<CharacterProperty> OnBeTriggerred;

    float time;

    public IObservable<CharacterProperty> OnBeTriggerredObservable()
    {
        return OnBeTriggerred ?? (OnBeTriggerred = new Subject<CharacterProperty>());
    }

    // Use this for initialization
    void Start () {
        time = 0;

    }
	
	// Update is called once per frame
	void Update () {
       
        if (time > Delay && target)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;

            Vector3 targetDirection = targetPosition - transform.position;
            float step = moveSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if(Vector3.Distance(transform.position, targetPosition) < TriggerDistance)
            {
                if (OnBeTriggerred != null)
                {
                    OnBeTriggerred.OnNext(target);
                }

                Destroy(gameObject);
            }
        }

        time += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CheckLayer != (CheckLayer | (1 << other.gameObject.layer)))
        {
            return;
        }

        target = other.gameObject.GetComponent<CharacterProperty>();
    }
}
