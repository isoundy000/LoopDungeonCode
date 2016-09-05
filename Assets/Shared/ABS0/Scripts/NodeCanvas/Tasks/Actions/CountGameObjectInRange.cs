using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("GameObject")]
    public class CountObjectInRange : ActionTask<Transform>
    {
        [TagField]
        [RequiredField]
        public BBParameter<string> searchTag;
        public BBParameter<float> searchRange;

        public BBParameter<bool> ignoreChildren;

        //[BlackboardOnly]
        //public BBParameter<GameObject> saveObjectAs;
        [BlackboardOnly]
        public BBParameter<int> saveCountAs;

        protected override string info
        {
            get { return "GetObject Counts With '" + searchTag; }
        }

        protected override void OnExecute()
        {

            var found = GameObject.FindGameObjectsWithTag(searchTag.value).ToList();
            if (found.Count == 0)
            {
                saveCountAs.value = 0;
                EndAction(false);
                return;
            }

            int count = 0;

            foreach (var go in found)
            {

                if (go.transform == agent)
                {
                    continue;
                }

                if (ignoreChildren.value && go.transform.IsChildOf(agent))
                {
                    continue;
                }

                var distance = Vector3.Distance(go.transform.position, agent.position);
                if (distance < searchRange.value)
                {
                    count++;
                }
            }

            saveCountAs.value = count;
            EndAction();
        }
    }
}