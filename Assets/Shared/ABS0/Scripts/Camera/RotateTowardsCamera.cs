using UnityEngine;
using System.Collections;

public class RotateTowardsCamera : MonoBehaviour {

    public Camera target;

    void Start()
    {
        if (!target)
        {
            target = Camera.main;
        }
    }

    void LateUpdate()
    {
        Quaternion cameraRotation = target.transform.rotation;
        // optionally ignore all but the y rotation, if you want it to be "square on" to the camera comment out the next line
        cameraRotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        transform.rotation = cameraRotation;
    }
}
