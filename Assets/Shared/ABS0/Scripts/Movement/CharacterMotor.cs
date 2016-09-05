using UnityEngine;
using System.Collections;

namespace ABS0
{

    public class CharacterMotor : MonoBehaviour
    {
        bool mMoveable = true;

        public bool IsMoveable()
        {
            return mMoveable;
        }

        public void SetMoveable(bool value)
        {
            mMoveable = value;
        }

        public virtual void Move(Vector3 velocity)
        {

        }

        public virtual void Stop()
        {

        }

        public virtual Vector3 GetVelocity()
        {
            return Vector3.zero;
        }
    }
}

