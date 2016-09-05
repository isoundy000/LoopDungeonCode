using UnityEngine;
using System.Collections;

using UniRx;
using UniRx.Triggers;
using System;

namespace ABS0
{

    public class UnityChanActionController : MonoBehaviour
    {

        //public enum Warrior
        //{
        //    Karate,
        //    Ninja,
        //    Brute,
        //    Sorceress,
        //    Knight,
        //    Mage,
        //    Archer,
        //    TwoHanded,
        //    Swordsman,
        //    Spearman,
        //    Hammer,
        //    Crossbow
        //}

        public GameObject DeathEffect;

        Animator mAnimator;
        public GameObject target;
        public GameObject weaponModel;
        public GameObject secondaryWeaponModel;
        float rotationSpeed = 15f;
        public float gravity = -9.83f;
        public float runSpeed = 8f;
        public float walkSpeed = 3f;
        public float strafeSpeed = 3f;
        bool canMove = true;
        //jumping variables
        public float jumpSpeed = 8f;
        bool jumpHold = false;
        bool canJump = true;
        float fallingVelocity = -2;
        bool isFalling = false;
        // Used for continuing momentum while in air
        public float inAirSpeed = 8f;
        float maxVelocity = 2f;
        float minVelocity = -2f;
        //platform variables
        Vector3 newVelocity;
        Vector3 inputVec;
        Vector3 targetDirection;
        Vector3 targetDashDirection;
        bool isDashing = false;
        bool isGrounded = true;
        bool dead = false;
        bool isStrafing;
        bool isBlocking = false;
        bool isStunned = false;
        bool isSitting = false;
        bool inBlock;
        bool blockGui;
        bool aimingGui;
        bool weaponSheathed;
        bool weaponSheathed2;
        bool isInAir;
        bool isStealth;
        public float stealthSpeed;
        bool isWall;
        bool ledgeGui;
        bool ledge;
        public float ledgeSpeed;
        int attack = 0;
        bool canChain;
        bool specialAttack2Bool;
        //public Warrior warrior;
        //private IKHands ikhands;


        bool mBlock;
        bool mKamae;

        CharacterMotor mMotor;
        ObservableStateMachineTrigger mObservableStateMachineTrigger;

        static int mIdleState = Animator.StringToHash("Base Layer.Idle");
        static int mWalkRunState = Animator.StringToHash("Base Layer.Movement.WalkRunBlend");
        static int mStrafeState = Animator.StringToHash("Base Layer.Movement.StrafeBlend");

        static int mLightHitState = Animator.StringToHash("Base Layer.Reactions.LightHit");
        static int mBlockState = Animator.StringToHash("Base Layer.Blocks.Block");

        static int mAttack1State = Animator.StringToHash("Base Layer.Attacks.Attack1");
        static int mAttack2State = Animator.StringToHash("Base Layer.Attacks.Attack2");
        static int mAttack3State = Animator.StringToHash("Base Layer.Attacks.Attack3");

        static int mSpecialAttack1State = Animator.StringToHash("Base Layer.Attacks.SpecialAttack1");

        static int mDeathState = Animator.StringToHash("Base Layer.Death.Death");
        static int mBlockCounterState = Animator.StringToHash("Base Layer.Blocks.BlockCounter");
        TargetingController mTargetingController;
        //private Collider[] mAutoTargets;

        CharacterProperty mCharacterProperty;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mMotor = GetComponent<CharacterMotor>();
            mCharacterProperty = GetComponent<CharacterProperty>();

            runSpeed = mCharacterProperty.WalkSpeed;

            mObservableStateMachineTrigger = mAnimator.GetBehaviour<ObservableStateMachineTrigger>();

            mObservableStateMachineTrigger
                .OnStateEnterAsObservable()
                .Subscribe(info =>
                {
                    OnEnterState(info.StateInfo);
                });

            mObservableStateMachineTrigger
               .OnStateExitAsObservable()
               .Subscribe(info =>
               {
                   OnExitState(info.StateInfo);
               });

            mTargetingController = GetComponent<TargetingController>();
            mTargetingController.CurrentTarget = target;
        }

        void OnEnterState(AnimatorStateInfo info)
        {
            if (info.fullPathHash == mBlockState ||
                info.fullPathHash == mLightHitState)
            {
                newVelocity = new Vector3(0, 0, 0);
                inputVec = new Vector3(0, 0, 0);
                UpdateMovement(info);
            }
            else if (info.fullPathHash == mAttack1State)
            {
                mAnimator.applyRootMotion = true;
            }
            else if (info.fullPathHash == mAttack2State)
            {
                mAnimator.applyRootMotion = true;
            }
            else if (info.fullPathHash == mAttack3State)
            {
                mAnimator.applyRootMotion = true;
            }
            else if (info.fullPathHash == mSpecialAttack1State)
            {
                //mAnimator.applyRootMotion = true;
                //newVelocity = new Vector3(0, 0, 0);
                //inputVec = new Vector3(0, 0, 0);
                //mAnimator.SetBool("Running", false);
                //mAnimator.SetBool("Moving", false);
                //mAnimator.SetFloat("Input X", 0);
                //mAnimator.SetFloat("Input Z", 0);
                //UpdateMovement();
                //mAttackActionInstance = new AttackActionInstance(AttackInfos[3]);
                //AttackInfos[3].targetInfo = new TargetInfo();
                //AttackInfos[3].targetInfo.first = target;
                //Lock(mAttackActionInstance.info);
                //OnAttack.OnNext(mAttackActionInstance.info);
            }
            else if (info.fullPathHash == mIdleState)
            {

            }
            else if (info.fullPathHash == mDeathState)
            {
                Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
                {
                    GameObject DeathEffectObj = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
                    Destroy(DeathEffectObj, 1.0f);
                    TeleportTo teleportTo = GetComponent<TeleportTo>();
                    if(teleportTo != null)
                    {
                        teleportTo.Teleport(()=>
                        {
                            mAnimator.SetBool("Died", false);
                            mAnimator.SetTrigger("ReviveTrigger");
                            mCharacterProperty.Revive();
                        });
                    }
                });

            }
        }

        void OnExitState(AnimatorStateInfo info)
        {

        }

        #region Updates

        void FixedUpdate()
        {
            AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

           // CheckForGrounded();
            //gravity
            //mRigidbody.AddForce(0, gravity, 0, ForceMode.Acceleration);
            //if (!isGrounded)
            //{
            //    AirControl();
            //}

            if (animatorStateInfo.fullPathHash == mIdleState ||
               animatorStateInfo.fullPathHash == mWalkRunState ||
               animatorStateInfo.fullPathHash == mStrafeState ||
               animatorStateInfo.fullPathHash == mBlockState ||
               animatorStateInfo.fullPathHash == mLightHitState)
            {
                UpdateMovement(animatorStateInfo);
            }

            //if (mRigidbody.velocity.y < fallingVelocity)
            //{
            //    isFalling = true;
            //}
            //else
            //{
            //    isFalling = false;
            //}
        }

        void LateUpdate()
        {
            AnimatorStateInfo mCurrentStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

            if (mCurrentStateInfo.fullPathHash == mIdleState ||
                mCurrentStateInfo.fullPathHash == mWalkRunState)
            {
                //Get local velocity of charcter
                Vector3 Velocity = mMotor.GetVelocity();
                float velocityXel = transform.InverseTransformDirection(Velocity).x;
                float velocityZel = transform.InverseTransformDirection(Velocity).z;
                //Update animator with movement values
                mAnimator.SetFloat("Input X", velocityXel / runSpeed);
                mAnimator.SetFloat("Input Z", velocityZel / runSpeed);
            }

            if(mCharacterProperty)
            {
                runSpeed = mCharacterProperty.WalkSpeed;
            }
        }

        void Update()
        {
            AnimatorStateInfo animatorStateInfo = mAnimator.GetCurrentAnimatorStateInfo(0);

            bool attackInput = ABS0TouchInput.Attack;
            bool magicInput = Input.GetButtonDown("magic");

            if (ABS0TouchInput.HeavyAttack)
            {
                mAnimator.SetTrigger("HeavyAttackTrigger");
            }

            mKamae = ABS0TouchInput.Kamae;

            if (!mAnimator.GetAnimatorTransitionInfo(0).anyState &&
                (animatorStateInfo.fullPathHash == mIdleState ||
                animatorStateInfo.fullPathHash == mWalkRunState ||
                animatorStateInfo.fullPathHash == mStrafeState))
            {
                attack = 1;
                mAnimator.SetInteger("Attack", attack);
                //update character position and facing
                UpdateMovement(animatorStateInfo);
                //InAir();
                //JumpingUpdate();

                Transform cameraTransform = Camera.main.transform;
                //Forward vector relative to the camera along the x-z plane   
                Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward = forward.normalized;
                //Right vector relative to the camera Always orthogonal to the forward vector
                Vector3 right = new Vector3(forward.z, 0, forward.x);
                //directional inputs

                float v = ABS0TouchInput.InputVector.y;
                float h = ABS0TouchInput.InputVector.x;
                float dv = ABS0TouchInput.DashVector.y;
                float dh = ABS0TouchInput.DashVector.x;
                // Target direction relative to the camera
                targetDirection = h * right + v * forward;
                // Target dash direction relative to the camera
                targetDashDirection = dh * right + dv * forward;
               
                inputVec = new Vector3(h, 0, v);

                if (ABS0TouchInput.Dash)
                {
                    if (!isDashing)
                    {
                        StartCoroutine(_DirectionalDash(ABS0TouchInput.InputVector.x,ABS0TouchInput.InputVector.y));
                    }
                } else
                {
                    //if there is some input (account for controller deadzone)
                    if ((v > .1 || v < -.1 || h > .1 || h < -.1))
                    {
                        mAnimator.applyRootMotion = false;
                        //set that character is moving
                        mAnimator.SetBool("Moving", true);
                        mAnimator.SetBool("Running", true);
                        //if targetting/strafing
                        if (mKamae)
                        {
                            isStrafing = true;
                            mAnimator.SetBool("Running", false);
                        }
                        else
                        {
                            isStrafing = false;
                            mAnimator.SetBool("Running", true);
                        }
                    }
                    else
                    {
                        //character is not moving
                        mAnimator.SetBool("Moving", false);
                        mAnimator.SetBool("Running", false);
                    }

                    //mAnimator.SetBool("Block", false);

                    if (!magicInput)
                    {
                        AttackChain(attackInput);
                    }
                    else
                    {
                        MagicChain();
                    }
                }
            }
            else if (animatorStateInfo.fullPathHash == mAttack1State ||
                animatorStateInfo.fullPathHash == mAttack2State ||
                animatorStateInfo.fullPathHash == mAttack3State ||
                 animatorStateInfo.fullPathHash == mBlockCounterState)
            {
                mAnimator.applyRootMotion = true;
                newVelocity = new Vector3(0, 0, 0);
                inputVec = new Vector3(0, 0, 0);
                mAnimator.SetBool("Running", false);
                mAnimator.SetBool("Moving", false);
                mAnimator.SetFloat("Input X", 0);
                mAnimator.SetFloat("Input Z", 0);
                AttackChain(attackInput);
            }
            else
            {
                newVelocity = new Vector3(0, 0, 0);
                inputVec = new Vector3(0, 0, 0);
            }

            mBlock = ABS0TouchInput.Block;

            if (mBlock)
            {
                mAnimator.SetBool("Block", true);
                mAnimator.SetBool("Running", false);
                mAnimator.SetBool("Moving", false);
                if (!isBlocking)
                {
                    isBlocking = true;
                    gameObject.AddComponent<JustGuardEffect>();
                }

            }
            else
            {
                if (isBlocking)
                {
                    isBlocking = false;
                    JustGuardEffect effect = gameObject.GetComponent<JustGuardEffect>();
                    Destroy(effect);
                }
                mAnimator.SetBool("Block", false);
            }
        }

        float UpdateMovement(AnimatorStateInfo info)
        {
            Vector3 motion = inputVec;

            if (isGrounded)
            {
                //if (!dead && !isBlocking && !blockGui && !isStunned)
                //{
                    //reduce input for diagonal movement
                    //motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
                    //character is not strafing
                    if (!isStrafing)
                    {
                        newVelocity = motion * runSpeed;
                    }
                    //character is strafing
                    else
                    {
                        newVelocity = motion * strafeSpeed;
                    }
                    if (ledge)
                    {
                        newVelocity = motion * ledgeSpeed;
                    }
                    if (isStealth)
                    {
                        newVelocity = motion * stealthSpeed;
                    }
                //}
                ////no input, character not moving
                //else
                //{
                //    newVelocity = new Vector3(0, 0, 0);
                //    inputVec = new Vector3(0, 0, 0);
                //}
            }
            //if character is falling
            else
            {
                newVelocity = mMotor.GetVelocity();
            }
            // limit velocity to x and z, by maintaining current y velocity:
            newVelocity.y = mMotor.GetVelocity().y;

            mMotor.Move(newVelocity);

            if(newVelocity.magnitude < 0.0001)
            {
                mMotor.Stop();
            }
            if (!isStrafing && !isWall)
            {
                //if not strafing, face character along input direction
                if (!ledgeGui || !ledge)
                {
                    RotateTowardMovementDirection();
                }
            }
            //if targetting button is held look at the target
            if (target != null && info.fullPathHash == mStrafeState)
            {
                Quaternion targetRotation;
                float rotationSpeed = 60f;
                targetRotation = Quaternion.LookRotation(target.transform.position - new Vector3(transform.position.x, 0, transform.position.z));
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
            }
            //return a movement value for the animator
            return inputVec.magnitude;
        }

        //face character along input direction
        void RotateTowardMovementDirection()
        {
            //if character is none of these things
            if (!dead && !blockGui && !isBlocking && !isStunned && inputVec != Vector3.zero && !isStrafing)
            {
                //take the camera orientated input vector and apply it to our characters facing with smoothing
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationSpeed);
            }
        }

        #endregion

        #region Jumping

        //void CheckForGrounded()
        //{
        //    float distanceToGround;
        //    float threshold = .45f;
        //    RaycastHit hit;
        //    Vector3 offset = new Vector3(0, .4f, 0);
        //    if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
        //    {
        //        distanceToGround = hit.distance;
        //        if (distanceToGround < threshold)
        //        {
        //            isGrounded = true;
        //            isInAir = false;
        //        }
        //        else
        //        {
        //            isGrounded = false;
        //            isInAir = true;
        //        }
        //    }
        //}

        //void JumpingUpdate()
        //{
        //    if (!jumpHold)
        //    {
        //        //If the character is on the ground
        //        if (isGrounded)
        //        {
        //            //set the animation back to idle
        //            mAnimator.SetInteger("Jumping", 0);
        //            canJump = true;
        //        }
        //        else
        //        {
        //            //character is falling
        //            if (!ledge)
        //            {
        //                if (isFalling)
        //                {
        //                    mAnimator.SetInteger("Jumping", 2);
        //                    canJump = false;
        //                }
        //            }
        //        }
        //    }
        //}

        //IEnumerator _Jump(float jumpTime)
        //{
        //    //Take the current character velocity and add jump movement
        //    mAnimator.SetTrigger("JumpTrigger");
        //    canJump = false;
        //    mRigidbody.velocity += jumpSpeed * Vector3.up;
        //    mAnimator.SetInteger("Jumping", 1);
        //    yield return new WaitForSeconds(jumpTime);
        //}

        //void AirControl()
        //{
        //    float x = Input.GetAxisRaw("Horizontal");
        //    float z = Input.GetAxisRaw("Vertical");
        //    Vector3 inputVec = new Vector3(x, 0, z);
        //    Vector3 motion = inputVec;
        //    //motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
        //    //allow some control the air
        //    mRigidbody.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
        //    //limit the amount of velocity we can achieve
        //    float velocityX = 0;
        //    float velocityZ = 0;
        //    if (mRigidbody.velocity.x > maxVelocity)
        //    {
        //        velocityX = mRigidbody.velocity.x - maxVelocity;
        //        if (velocityX < 0)
        //        {
        //            velocityX = 0;
        //        }
        //        mRigidbody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
        //    }
        //    if (mRigidbody.velocity.x < minVelocity)
        //    {
        //        velocityX = mRigidbody.velocity.x - minVelocity;
        //        if (velocityX > 0)
        //        {
        //            velocityX = 0;
        //        }
        //        mRigidbody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
        //    }
        //    if (mRigidbody.velocity.z > maxVelocity)
        //    {
        //        velocityZ = mRigidbody.velocity.z - maxVelocity;
        //        if (velocityZ < 0)
        //        {
        //            velocityZ = 0;
        //        }
        //        mRigidbody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
        //    }
        //    if (mRigidbody.velocity.z < minVelocity)
        //    {
        //        velocityZ = mRigidbody.velocity.z - minVelocity;
        //        if (velocityZ > 0)
        //        {
        //            velocityZ = 0;
        //        }
        //        mRigidbody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
        //    }
        //}

        //void InAir()
        //{
        //    if (isInAir)
        //    {
        //        if (ledgeGui)
        //        {
        //            mAnimator.SetTrigger("Ledge-Catch");
        //            ledge = true;
        //        }
        //    }
        //}

        #endregion

        #region Buttons

        void AttackChain(bool attackInput)
        {
            if (attackInput)
            {
                Collider[] mAutoTargets = Physics.OverlapSphere(transform.position, 2, 1 << LayerMask.NameToLayer("Enemy"));

                GameObject target = null;
                if (mAutoTargets != null)
                {
                    var dist = Mathf.Infinity;
                    foreach (var autoTarget in mAutoTargets)
                    {
                        if(!autoTarget.tag.Equals("Enemy"))
                        {
                            continue;
                        }
                        var newDist = Vector3.Distance(autoTarget.transform.position, transform.position);
                        if (newDist < dist)
                        {
                            dist = newDist;
                            target = autoTarget.gameObject;
                        }
                    }
                }

                if(target != null)
                {
                    transform.LookAt(target.transform);
                }
                mAnimator.SetTrigger("LightAttackTrigger");
            }
        }

        private void MagicChain()
        {
            //if (mAttackActionInstance == null)
            //{
                mAnimator.SetTrigger("SpecialAttack1Trigger");
            //}
        }

        //void Lock(AttackActionInfo info)
        //{
        //    if (mAttackDisposables != null)
        //    {
        //        mAttackDisposables.Dispose();
        //    }

        //    mAttackDisposables = new SingleAssignmentDisposable();
        //    mAttackDisposables.Disposable = Observable.Timer(TimeSpan.FromSeconds(info.lifeTime)).Subscribe(_ =>
        //    {
        //        attack = 0;
        //        mAttackActionInstance = null;
        //        mAnimator.applyRootMotion = false;
        //        mAnimator.SetInteger("Attack", 0);
        //        mAttackDisposables.Dispose();
        //        mAttackDisposables = null;
        //    });
        //}

        //IEnumerator _JumpAttack1()
        //{
            //yield return new WaitForFixedUpdate();
            //jumpHold = true;
            //mRigidbody.velocity += jumpSpeed * -Vector3.up;
            //mAnimator.SetTrigger("JumpAttack1Trigger");
            //if (warrior == Warrior.Knight)
            //{
            //    StartCoroutine(_LockMovementAndAttack(1f));
            //    yield return new WaitForSeconds(.5f);
            //}
            //jumpHold = false;
        //}

        #endregion

        #region Dashing

        IEnumerator _DirectionalDash(float x, float v)
        {
            //check which way the dash is pressed relative to the character facing
            float angle = Vector3.Angle(targetDashDirection, -transform.forward);
            float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(targetDashDirection, transform.forward)));
            // angle in [-179,180]
            float signed_angle = angle * sign;
            //angle in 0-360
            float angle360 = (signed_angle + 180) % 360;
            //deternime the animation to play based on the angle
            if (angle360 > 315 || angle360 < 45)
            {
                StartCoroutine(_Dash2(1));
            }
            if (angle360 > 45 && angle360 < 135)
            {
                StartCoroutine(_Dash2(2));
            }
            if (angle360 > 135 && angle360 < 225)
            {
                StartCoroutine(_Dash2(3));
            }
            if (angle360 > 225 && angle360 < 315)
            {
                StartCoroutine(_Dash2(4));
            }
            yield return null;
        }

        IEnumerator _Dash(int dashDirection)
        {
            isDashing = true;
            mAnimator.SetInteger("Dash", dashDirection);
            StartCoroutine(_LockMovementAndAttack(1.15f));
            yield return new WaitForSeconds(.1f);
            mAnimator.SetInteger("Dash", 0);
            isDashing = false;
            ABS0TouchInput.Dash = false;
        }

        IEnumerator _Dash2(int dashDirection)
        {
            isDashing = true;
            mAnimator.SetInteger("Dash2", dashDirection);
            yield return new WaitForEndOfFrame();
            mAnimator.SetInteger("Dash2", dashDirection);
            yield return new WaitForEndOfFrame();
            StartCoroutine(_LockMovementAndAttack(.65f));
            mAnimator.SetInteger("Dash2", 0);
            yield return new WaitForSeconds(.95f);
            isDashing = false;
            ABS0TouchInput.Dash = false;
        }

        #endregion

        #region Misc

        IEnumerator _SetInAir(float timeToStart, float lenthOfTime)
        {
            yield return new WaitForSeconds(timeToStart);
            isInAir = true;
            yield return new WaitForSeconds(lenthOfTime);
            isInAir = false;
        }

        public IEnumerator _ChainWindow(float timeToWindow, float chainLength)
        {
            yield return new WaitForSeconds(timeToWindow);
            canChain = true;
            mAnimator.SetInteger("Attack", 0);
            yield return new WaitForSeconds(chainLength);
            canChain = false;
        }

        IEnumerator _LockMovementAndAttack(float pauseTime)
        {
            isStunned = true;
            mAnimator.applyRootMotion = true;
            inputVec = new Vector3(0, 0, 0);
            newVelocity = new Vector3(0, 0, 0);
            mAnimator.SetFloat("Input X", 0);
            mAnimator.SetFloat("Input Z", 0);
            mAnimator.SetBool("Moving", false);
            yield return new WaitForSeconds(pauseTime);
            mAnimator.SetInteger("Attack", 0);
            canChain = false;
            isStunned = false;
            mAnimator.applyRootMotion = false;
            //small pause to let blending finish
            yield return new WaitForSeconds(.2f);
            attack = 0;
        }

        void SheathWeapon()
        {
            mAnimator.SetTrigger("WeaponSheathTrigger");
            StartCoroutine(_LockMovementAndAttack(1.4f));
            StartCoroutine(_WeaponVisibility(.5f, false));
            weaponSheathed = true;
        }

        void UnSheathWeapon()
        {
            mAnimator.SetTrigger("WeaponUnsheathTrigger");
            StartCoroutine(_WeaponVisibility(.6f, true));
            StartCoroutine(_LockMovementAndAttack(1.4f));
            weaponSheathed = false;
        }

        IEnumerator _WeaponVisibility(float waitTime, bool weaponVisiblity)
        {
            yield return new WaitForSeconds(waitTime);
            weaponModel.SetActive(weaponVisiblity);
        }

        IEnumerator _SecondaryWeaponVisibility(float waitTime, bool weaponVisiblity)
        {
            yield return new WaitForSeconds(waitTime);
            secondaryWeaponModel.GetComponent<Renderer>().enabled = weaponVisiblity;
        }

        IEnumerator _SetLayerWeight(float time)
        {
            mAnimator.SetLayerWeight(1, 1);
            yield return new WaitForSeconds(time);
            float a = 1;
            for (int i = 0; i < 20; i++)
            {
                a -= .05f;
                mAnimator.SetLayerWeight(1, a);
                yield return new WaitForEndOfFrame();
            }
            mAnimator.SetLayerWeight(1, 0);
        }

        IEnumerator _BlockHitReact()
        {
            StartCoroutine(_LockMovementAndAttack(.5f));
            mAnimator.SetTrigger("BlockHitReactTrigger");
            yield return null;
        }

        IEnumerator _BlockBreak()
        {
            StartCoroutine(_LockMovementAndAttack(1f));
            mAnimator.SetTrigger("BlockBreakTrigger");
            yield return null;
        }

        IEnumerator _GetHit()
        {
            mAnimator.SetTrigger("LightHitTrigger");
            StartCoroutine(_LockMovementAndAttack(2.8f));
            yield return null;
        }

        void Dead()
        {
            mAnimator.applyRootMotion = true;
            mAnimator.SetTrigger("DeathTrigger");
            dead = true;
        }

        IEnumerator _Revive()
        {
            mAnimator.SetTrigger("ReviveTrigger");
            StartCoroutine(_LockMovementAndAttack(1.1f));
            yield return null;
            dead = false;
        }

        #endregion
    }
}