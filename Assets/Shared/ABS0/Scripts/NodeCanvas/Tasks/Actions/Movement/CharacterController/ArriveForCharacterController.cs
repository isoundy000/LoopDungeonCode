using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class ArriveForCharacterController : ActionTask<CharacterController>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopDistance = 0.1f;
        public BBParameter<float> slowDownDistance = 2.0f;
        public bool ignoreY;
        public bool repeat;

        protected override void OnExecute() { Move(); }
        protected override void OnUpdate() { Move(); }

        void Move()
        {
            float distance = (agent.transform.position - target.value.transform.position).magnitude;
            float targetSpeed = 0.0f;

            if (distance > slowDownDistance.value)
            {
                targetSpeed = speed.value;
            }
            else
            {
                targetSpeed = speed.value * distance / slowDownDistance.value;
            }

            if (distance > stopDistance.value)
            {
                Quaternion rotation = Quaternion.LookRotation(target.value.transform.position - agent.transform.position);
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * rotateSpeed.value);
                if (ignoreY)
                {
                    agent.Move(new Vector3(agent.transform.forward.x, 0, agent.transform.forward.z) * targetSpeed * Time.deltaTime);
                }
                else
                {
                    agent.Move(agent.transform.forward * targetSpeed * Time.deltaTime);
                }
            }
            else if (!repeat)
            {
                EndAction();
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (agent != null)
            {
                Gizmos.DrawSphere(target.value.transform.position, slowDownDistance.value);
                Gizmos.DrawSphere(target.value.transform.position, stopDistance.value);
            }
        }
    }

}