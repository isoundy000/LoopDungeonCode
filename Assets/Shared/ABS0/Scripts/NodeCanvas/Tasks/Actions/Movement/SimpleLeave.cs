using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class SimpleLeave : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopSpeed = 0.1f;

        public BBParameter<float> slowDownDistance = 2.0f;
        public BBParameter<float> maxDistance = 5.0f;

        public bool repeat;

        protected override void OnExecute() { Move(); }
        protected override void OnUpdate() { Move(); }

        void Move()
        {
            float distance = (agent.position - target.value.transform.position).magnitude;

            float targetSpeed = speed.value;

            if (distance <= slowDownDistance.value)
            {
                targetSpeed = speed.value;
            }
            else
            {
                targetSpeed = targetSpeed - speed.value * distance / maxDistance.value;
            }

            if (targetSpeed > stopSpeed.value)
            {
                Quaternion rotation = Quaternion.LookRotation(agent.position - target.value.transform.position);
                agent.rotation = Quaternion.Slerp(agent.rotation, rotation, Time.deltaTime * rotateSpeed.value);
                agent.position = agent.position + new Vector3(agent.forward.x, agent.forward.y, agent.forward.z) * targetSpeed * Time.deltaTime;
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
                Gizmos.DrawSphere(target.value.transform.position, maxDistance.value);
            }
        }
    }
}