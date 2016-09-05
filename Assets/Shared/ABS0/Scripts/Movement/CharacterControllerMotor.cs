using UnityEngine;
using System.Collections;
namespace ABS0
{

    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerMotor : CharacterMotor
    {
        bool mStopped = true;
        Vector3 mVelocity;

        CharacterController mCharacterController;
        // Use this for initialization
        void Start()
        {
            mCharacterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

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
            base.Move(velocity);
            mStopped = false;
            mCharacterController.SimpleMove(velocity);
            mVelocity = velocity;
        }

        public override void Stop()
        {
            base.Stop();
            mStopped = true;
            mVelocity = Vector3.zero;
        }
    }
}

