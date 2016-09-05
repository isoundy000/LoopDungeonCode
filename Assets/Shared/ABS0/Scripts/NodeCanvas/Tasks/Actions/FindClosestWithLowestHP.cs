using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [Category("GameObject")]
    [Description("Find the closest game object of tag to the agent")]
    public class FindClosestWithLowestHP : ActionTask<Transform>
    {

        [TagField]
        [RequiredField]
        public BBParameter<string> searchTag;
        public BBParameter<bool> ignoreChildren;
        public BBParameter<float> maxDistance;

        [BlackboardOnly]
        public BBParameter<GameObject> saveObjectAs;
        [BlackboardOnly]
        public BBParameter<float> saveDistanceAs;

        protected override void OnExecute()
        {

            var found = GameObject.FindGameObjectsWithTag(searchTag.value).ToList();
            if (found.Count == 0)
            {
                saveObjectAs.value = null;
                saveDistanceAs.value = 0;
                EndAction(false);
                return;
            }

            GameObject lowest = null;
            var hp = Mathf.Infinity;
            float distance = float.PositiveInfinity;
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
               
                distance = Vector3.Distance(go.transform.position, agent.position);

                if(distance > maxDistance.value)
                {
                    continue;
                }

                CharacterProperty mProperty = go.GetComponent<CharacterProperty>();

                if (mProperty == null || mProperty.PersentHP > 0.9f)
                {
                    continue;
                }

                var newHp = mProperty.PersentHP;
                if (newHp < hp)
                {
                    hp = newHp;
                    lowest = go;
                }
            }

            if(lowest == null)
            {
                saveObjectAs.value = null;
                saveDistanceAs.value = 0;
                EndAction(false);
                return;
            } else
            {

                saveObjectAs.value = lowest;
                saveDistanceAs.value = distance;
                EndAction();
            }

        }
    }
}