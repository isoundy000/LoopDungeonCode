using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	[Description("Makes the agent wander randomly")]
	public class WanderForCharacterController : ActionTask<CharacterController> {

        public BBParameter<float> speed = 2;
        public BBParameter<float> rotateSpeed = 2;
        [SliderField(0.1f, 10)]
        public BBParameter<float> stopDistance = 0.1f;
    
        public BBParameter<float> minWanderDistance = 5;
		public BBParameter<float> maxWanderDistance = 20;

        public bool ignoreY;
        public bool repeat = true;

        bool idle = true;
        Vector3 wanderPos;

		protected override void OnExecute(){
            DoWander();
		}
		protected override void OnUpdate(){
            DoWander();
        }

		void DoWander(){
            if(idle) {
                maxWanderDistance.value = Mathf.Max(minWanderDistance.value, maxWanderDistance.value);
                minWanderDistance.value = Mathf.Min(minWanderDistance.value, maxWanderDistance.value);
                do
                {
                    Vector2 offset = (Random.insideUnitCircle * maxWanderDistance.value);
                    wanderPos = agent.transform.position + new Vector3(offset.x, 0, offset.y);
                }
                while ((wanderPos - agent.transform.position).sqrMagnitude < minWanderDistance.value);       
                
            }

            if ((agent.transform.position - wanderPos).magnitude > stopDistance.value)
            {
                idle = false;
                Move(wanderPos);
            }
            else 
            {
                idle = true;
                if (!repeat)
                {
                    EndAction();
                }
            }
        }

        void Move(Vector3 targetPos)
        {
            Quaternion rotation = Quaternion.LookRotation(targetPos - agent.transform.position);
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


        protected override void OnPause(){ OnStop(); }
		protected override void OnStop(){
//			if (agent.gameObject.activeSelf)
//				agent.ResetPath();
		}
	}
}