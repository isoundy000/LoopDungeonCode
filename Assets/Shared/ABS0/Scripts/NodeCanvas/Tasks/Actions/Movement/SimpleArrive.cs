using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	public class SimpleArrive : ActionTask<Transform> {

		[RequiredField]
		public BBParameter<GameObject> target;
		public BBParameter<float> speed = 2;
		public BBParameter<float> rotateSpeed = 2;
		[SliderField(0.1f, 10)]
		public BBParameter<float> stopDistance = 0.1f;
		public BBParameter<float> slowDownDistance = 2.0f;
		public bool repeat;

		protected override void OnExecute(){Move();}
		protected override void OnUpdate(){Move();}

		void Move(){
			float distance = (agent.position - target.value.transform.position).magnitude;
			float targetSpeed = 0.0f;

			if (distance > slowDownDistance.value) {
				targetSpeed = speed.value;
			} else {
				targetSpeed = speed.value * distance / slowDownDistance.value;
			}

			if ( distance > stopDistance.value ){
				Quaternion rotation = Quaternion.LookRotation (target.value.transform.position - agent.position);
				agent.rotation = Quaternion.Slerp (agent.rotation, rotation, Time.deltaTime * rotateSpeed.value);
				agent.position = agent.position + new Vector3 (agent.forward.x, agent.forward.y, agent.forward.z) * targetSpeed * Time.deltaTime;
			} else if (!repeat){
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