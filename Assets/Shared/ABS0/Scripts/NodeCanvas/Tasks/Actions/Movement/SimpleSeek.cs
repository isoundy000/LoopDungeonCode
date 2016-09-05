using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	public class SimpleSeek : ActionTask<Transform> {

		[RequiredField]
		public BBParameter<GameObject> target;
		public BBParameter<float> speed = 2;
		public BBParameter<float> rotateSpeed = 2;
		[SliderField(0.1f, 10)]
		public BBParameter<float> stopDistance = 0.1f;
		public bool repeat;

		protected override void OnExecute(){Move();}
		protected override void OnUpdate(){Move();}

		void Move(){
			if ( (agent.position - target.value.transform.position).magnitude > stopDistance.value ){
				Quaternion rotation = Quaternion.LookRotation (target.value.transform.position - agent.position);
				agent.rotation = Quaternion.Slerp (agent.rotation, rotation, Time.deltaTime * rotateSpeed.value);
				agent.position = agent.position + new Vector3 (agent.forward.x, agent.forward.y, agent.forward.z) * speed.value * Time.deltaTime;
			} else if (!repeat){
				EndAction();
			}
		}
	}
}