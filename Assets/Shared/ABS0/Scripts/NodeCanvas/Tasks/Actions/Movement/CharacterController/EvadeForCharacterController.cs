using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class EvadeForCharacterController : ActionTask<CharacterController>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopDistance = 1.0f;
        public BBParameter<float> maxPredictionDistance = 3f;
        public bool ignoreY;
        public bool repeat;
        [BlackboardOnly]
        public BBParameter<float> saveAs;

        Vector3 predictionPosition;
        Vector3 lastPosition;

        protected override void OnExecute()
        {
            if (target.value == null)
            {
                EndAction(false);
                return;
            }
            predictionPosition = target.value.transform.position;
            Move();
        }

        protected override void OnUpdate() { Move(); }

        void Move()
        {
            if (target.value == null)
            {
                EndAction(false);
                return;
            }
            saveAs.value = (agent.transform.position - predictionPosition).magnitude;
            if (saveAs.value < stopDistance.value)
            {
                Transform targetTransform = target.value.transform;
                Quaternion rotation = Quaternion.LookRotation(agent.transform.position - predictionPosition);
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * rotateSpeed.value);

                if (ignoreY)
                {
                    agent.Move(new Vector3(agent.transform.forward.x, 0, agent.transform.forward.z) * speed.value * Time.deltaTime);
                }
                else
                {
                    agent.Move(agent.transform.forward * speed.value * Time.deltaTime);
                }

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
                Gizmos.DrawLine(target.value.transform.position, predictionPosition);
            }
        }
    }

}