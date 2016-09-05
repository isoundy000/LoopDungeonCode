using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class SimpleRotateTowards : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        [SliderField(1, 180)]
        public BBParameter<float> angleDifference = 5;
        public bool repeat;

        public bool ignoreY = true;

        protected override void OnExecute() { Rotate(); }
        protected override void OnUpdate() { Rotate(); }

        void Rotate()
        {
            if(target.value == null)
            {
                EndAction(false);
                return;
            }

            Vector3 targetPosition = target.value.transform.position;
            Vector3 agentPosition = agent.position;

            if(ignoreY)
            {
                targetPosition.y = 0;
                agentPosition.y = 0;
            }

            if (Vector3.Angle(targetPosition - agentPosition, agent.forward) > angleDifference.value)
            {
                var dir = targetPosition - agentPosition;
                agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0));
            }
            else
            {
                if (!repeat)
                    EndAction();
            }
        }
    }
}