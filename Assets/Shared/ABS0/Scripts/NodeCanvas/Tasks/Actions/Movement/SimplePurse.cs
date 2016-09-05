using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class SimplePurse : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopDistance = 0.1f;
        public BBParameter<float> maxPredictionDistance = 3f;
        [BlackboardOnly]
        public BBParameter<float> saveAs;
        public bool repeat;

        Vector3 predictionPosition;
        Vector3 lastPosition;

        protected override void OnExecute() {
            predictionPosition = target.value.transform.position;
            Move();
        }
        protected override void OnUpdate() { Move(); }

        void Move()
        {
            if (target == null || target.value == null)
            {
                EndAction();
                return;
            }
            saveAs.value = (agent.position - predictionPosition).magnitude;
            if (saveAs.value > stopDistance.value)
            {
                Transform targetTransform = target.value.transform;
                Quaternion rotation = Quaternion.LookRotation(predictionPosition - agent.position);
                agent.rotation = Quaternion.Slerp(agent.rotation, rotation, Time.deltaTime * rotateSpeed.value);
                agent.position = agent.position + agent.forward * speed.value * Time.deltaTime;

                Vector3 v = (targetTransform.position - lastPosition) / Time.deltaTime;
                v.Normalize();
                predictionPosition = targetTransform.position + v * maxPredictionDistance.value;
                lastPosition = targetTransform.position;
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
                Gizmos.DrawSphere(predictionPosition, 1.0f);
                Gizmos.DrawLine(agent.position, predictionPosition);
            }
        }
    }
}