using UnityEngine;
using System.Collections;
using UniRx;
using System;

namespace ABS0
{
    public class AttackController : MonoBehaviour
    {

        AnimatorController mActionController;

        public AttackCollider[] mAttackCollider;

        SingleAssignmentDisposable[] mAttackColliderDisposables;

        // Use this for initialization
        void Start()
        {
            mAttackColliderDisposables = new SingleAssignmentDisposable[mAttackCollider.Length];
            mActionController = GetComponent<AnimatorController>();

            mActionController.OnHitStartAsObservable.Subscribe(info =>
            {
                if(info.colliderId < 0)
                {
                    return;
                }

                mAttackCollider[info.colliderId].AttackActionInfo = info;
            });

            mActionController.OnHitEndAsObservable.Subscribe(info =>
            {
                if (info.colliderId < 0)
                {
                    return;
                }

                mAttackCollider[info.colliderId].AttackActionInfo = null;
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

