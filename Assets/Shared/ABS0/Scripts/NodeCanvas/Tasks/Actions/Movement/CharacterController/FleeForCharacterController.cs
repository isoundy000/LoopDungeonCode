using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{

    [Category("Movement")]
    public class FleeForCharacterController : ActionTask<CharacterController>
    {

        [RequiredField]
        public BBParameter<GameObject> target;
        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopDistance = 1.0f;
        public bool ignoreY;
        public bool repeat;

        protected override void OnExecute() { Move(); }
        protected override void OnUpdate() { Move(); }

        void Move()
        {
            if ((agent.transform.position - target.value.transform.position).magnitude < stopDistance.value)
            {
                Quaternion rotation = Quaternion.LookRotation(agent.transform.position - target.value.transform.position);
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * rotateSpeed.value);
                if (ignoreY)
                {
                    agent.Move(new Vector3(agent.transform.forward.x, 0, agent.transform.forward.z) * speed.value * Time.deltaTime);
                }
                else
                {
                    agent.Move(agent.transform.forward * speed.value * Time.deltaTime);
                }
            }
            else if (!repeat)
            {
                EndAction();
            }
        }
    }
}