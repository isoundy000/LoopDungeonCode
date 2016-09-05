using UnityEngine;
using System.Collections;
using System;
using UniRx;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour
{
    public Image StartIndicator;
    public Image EndIndicator;

    Canvas mCanvas;

    float downTime;
    // Use this for initialization
    void Start()
    {
        mCanvas = GetComponent<Canvas>();
        StartIndicator.enabled = false;
        EndIndicator.enabled = false;

        TouchBeganStream
            .Timestamp()
            .Subscribe(t => {
                int fingerId = t.Value.fingerId;
                Vector2 startPosition = t.Value.position;
                StickStart(startPosition);
                Observable.EveryUpdate()
                        .Where(_ => !IsTouchedOnUI())
                        .SelectMany(_ => Input.touches)
                        .SkipWhile(_ => fingerId == -1)
                        .Where(t0 => (t0.fingerId == t.Value.fingerId))
                        .Where(t0 => t0.phase != TouchPhase.Ended && t0.phase != TouchPhase.Canceled)
                        .First()
                        .Timestamp()
                        .Subscribe(t1 => {
                            StickMove(t1.Value.position);
                        }, () => {
                            fingerId = -1;
                            StickEnd();
                        });
            });


        Observable.EveryUpdate()
            .SelectMany(_ => Input.touches)
            .Where(t => t.phase == TouchPhase.Began)
            .Timestamp()
            .Subscribe(t => {
                Vector2 startPosition = t.Value.position;
                Observable.EveryUpdate()
                    .SelectMany(_ => Input.touches)
                    .Where(_ => !IsTouchedOnUI())
                    .SkipWhile(t1 => t1.fingerId != t.Value.fingerId)
                    .Where(t1 => t1.phase == TouchPhase.Ended || t1.phase == TouchPhase.Canceled)
                    .First()
                    .Timestamp()
                    .Subscribe(t2 => {
                        double deltaTime = (t2.Timestamp - t.Timestamp).TotalSeconds;
                        if (deltaTime < 0.5)
                        {
                            if(Vector2.Distance(t2.Value.position, startPosition) > 3)
                            {
                                ABS0TouchInput.Attack = false;
                                ABS0TouchInput.Dash = true;
                            } else
                            {
                                ABS0TouchInput.Attack = true;
                            }
                            
                        }
                        else
                        {
                            ABS0TouchInput.Attack = false;
                        }
                    }, () => {
                        Debug.Log("exit");
                    });
            });


        //.Scan ((x, y) => (y.Value.fingerId == x.Value.fingerId) ? y : x)

        //			.TakeUntil( Observable.Timer(TimeSpan.FromSeconds(0.5f) ) )
        //			.Subscribe (t => {
        //				Debug.Log(t.Value.fingerId + "<>" +t.Value.phase);
        //			},()=>{
        //				Debug.Log("touch timeout");
        //			});

        //			.Subscribe (t => {
        //				GetTouchEndOrCancelStream(t.Value.fingerId).Timestamp()
        //					.Subscribe( tt => {
        //						if( (tt.Timestamp - t.Timestamp).TotalSeconds < 0.5) {
        //							Attack = true;
        //						}
        //				});
        //		});

    }

    bool IsTouchedOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static IObservable<Touch> TouchBeganStream
    {
        get
        {
            return Observable.EveryUpdate()
                .SelectMany(_ => Input.touches)
                .Where(touch => touch.phase == TouchPhase.Began);
        }
    }

    public static IObservable<Touch> GetTouchEndOrCancelStream(int fingerId)
    {
        return Observable.EveryUpdate()
            .SelectMany(_ => Input.touches)
            .Where(t => (t.fingerId == fingerId))
            .TakeWhile(t => t.phase != TouchPhase.Ended && t.phase != TouchPhase.Canceled);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StickStart(Vector2 inputPosition)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, inputPosition, mCanvas.worldCamera, out position);
        StartIndicator.transform.position = mCanvas.transform.TransformPoint(position);
        EndIndicator.transform.position = StartIndicator.transform.position;
        StartIndicator.enabled = true;
        EndIndicator.enabled = true;
    }

    void StickMove(Vector2 inputPosition)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, inputPosition, mCanvas.worldCamera, out position);
        EndIndicator.transform.position = mCanvas.transform.TransformPoint(position);
        ABS0TouchInput.InputVector = Vector3.Normalize(EndIndicator.transform.position - StartIndicator.transform.position);
    }

    void StickEnd()
    {
        EndIndicator.transform.position = StartIndicator.transform.position;
        StartIndicator.enabled = false;
        EndIndicator.enabled = false;
        ABS0TouchInput.InputVector = Vector3.zero;
    }
}
