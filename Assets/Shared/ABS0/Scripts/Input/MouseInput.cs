using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour {

    public Image StartIndicator;
    public Image EndIndicator;

    Canvas mCanvas;

    float downTime;
    // Use this for initialization
    void Start () {
        mCanvas = GetComponent<Canvas>();
        StartIndicator.enabled = false;
        EndIndicator.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        ABS0TouchInput.InputVector = Vector3.zero;
        ABS0TouchInput.Attack = false;
        ABS0TouchInput.HeavyAttack = false;
        ABS0TouchInput.Block = false;
        ABS0TouchInput.Kamae = false;

        if (Input.GetMouseButtonDown(0))
        {
            ABS0TouchInput.Dash = false;

            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, Input.mousePosition, mCanvas.worldCamera, out position);
            StartIndicator.transform.position = mCanvas.transform.TransformPoint(position);
            EndIndicator.transform.position = StartIndicator.transform.position;
            StartIndicator.enabled = true;
            EndIndicator.enabled = true;

            downTime = Time.time;

        }
        else if (Input.GetMouseButton(0))
        {
            if((Time.time - downTime) >= 0.2)
            {
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, Input.mousePosition, mCanvas.worldCamera, out position);
                EndIndicator.transform.position = mCanvas.transform.TransformPoint(position);
                Vector3 delta = mCanvas.transform.TransformPoint(position) - StartIndicator.transform.position;

                ABS0TouchInput.InputVector = Vector3.Normalize(EndIndicator.transform.position - StartIndicator.transform.position);

                if ((Time.time - downTime) >= 0.5 && delta.magnitude < 1)
                {
                    ABS0TouchInput.Block = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.transform as RectTransform, Input.mousePosition, mCanvas.worldCamera, out position);
            Vector3 delta = mCanvas.transform.TransformPoint(position) - StartIndicator.transform.position;
            ABS0TouchInput.InputVector = Vector3.Normalize(delta);

            StartIndicator.transform.position = mCanvas.transform.TransformPoint(position);
            EndIndicator.transform.position = StartIndicator.transform.position;
            StartIndicator.enabled = false;
            EndIndicator.enabled = false;


            if(delta.magnitude < 100)
            {
                if ((Time.time - downTime) < 0.2)
                {
                    ABS0TouchInput.Attack = true;
                }
                else if ((Time.time - downTime) > 0.2 && (Time.time - downTime) < 0.5)
                {
                    ABS0TouchInput.HeavyAttack = true;
                }
            }
            else
            {
                if ((Time.time - downTime) < 0.2)
                {
                    ABS0TouchInput.Dash = true;
                    ABS0TouchInput.DashVector = ABS0TouchInput.InputVector;
                }
            }
            

            ABS0TouchInput.InputVector = Vector3.zero;

            ABS0TouchInput.Block = false;
        }
    }
}
