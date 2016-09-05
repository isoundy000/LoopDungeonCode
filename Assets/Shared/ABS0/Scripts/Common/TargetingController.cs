using UnityEngine;
using System.Collections;
using UniRx;

public class TargetingController : MonoBehaviour {

    GameObject mCurrentTarget;

    SingleAssignmentDisposable mOnDiedSubscription;

    public GameObject CurrentTarget
    {
        get
        {
            return (mCurrentTarget == null) ? null : mCurrentTarget;
        }

        set
        {
            if (value != mCurrentTarget)
            {
                if (mOnDiedSubscription != null)
                {
                    mOnDiedSubscription.Dispose();
                }

                mOnDiedSubscription = new SingleAssignmentDisposable();

                mCurrentTarget = value;

                if (mCurrentTarget != null)
                {
                    CharacterProperty characterProperty = mCurrentTarget.GetComponent<CharacterProperty>();

                    if (characterProperty != null)
                    {
                        mOnDiedSubscription.Disposable = characterProperty.OnDiedAsObservable.Subscribe(_ =>
                        {
                            mCurrentTarget = null;
                        });
                    }
                    else
                    {
                        mCurrentTarget = null;
                    }
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public bool HasAliveTarget()
    {
        if(mCurrentTarget == null)
        {
            return false;
        } else
        {
            CharacterProperty characterProperty = mCurrentTarget.GetComponent<CharacterProperty>();
            if(characterProperty)
            {
                return characterProperty.IsAlive;
            } else
            {
                return false;
            }
        }
    }

#if UNITY_EDITOR

    public void OnDrawGizmos()
    {
        if (mCurrentTarget != null)
        {

            Vector3 positionOffset = Vector3.zero;
            var oldColor = UnityEditor.Handles.color;
            var color = Color.red;
            color.a = 0.2f;
            UnityEditor.Handles.color = color;

            var beginDirection = Quaternion.AngleAxis(360, Vector3.up) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(mCurrentTarget.transform.position, transform.up, beginDirection, 360, 1.0f);

            UnityEditor.Handles.DrawLine(transform.position, mCurrentTarget.transform.position);

            UnityEditor.Handles.color = oldColor;
        }

    }
#endif
}
