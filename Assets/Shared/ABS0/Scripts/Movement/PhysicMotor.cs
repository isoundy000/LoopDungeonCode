using UnityEngine;
using System.Collections;
using ABS0;

[RequireComponent(typeof(Rigidbody))]
public class PhysicMotor : CharacterMotor
{

    bool mStopped = true;
    Vector3 mVelocity;

    Rigidbody mController;
    // Use this for initialization
    void Start()
    {
        mController = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        mController.AddForce(Physics.gravity, ForceMode.Acceleration);
    }

    void LateUpdate()
    {

    }

    public override Vector3 GetVelocity()
    {
        return mStopped ? Vector3.zero : mVelocity;
    }

    public override void Move(Vector3 velocity)
    {
        if(!mController)
        {
            return;
        }
        base.Move(velocity);
        mStopped = false;
       
        if (velocity.magnitude < 0.0001)
        {
            mController.velocity = Vector3.zero;
            mController.Sleep();
        } else
        {
            mController.velocity = velocity;
        }
        mVelocity = velocity;
    }

    public override void Stop()
    {
        if(!mController)
        {
            return;
        }
        base.Stop();
        mStopped = true;
        mVelocity = Vector3.zero;
        mController.velocity = Vector3.zero;
        mController.Sleep();
    }
}
