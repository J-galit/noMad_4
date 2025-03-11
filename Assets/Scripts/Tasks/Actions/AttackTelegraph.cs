using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class AttackTelegraph : ActionTask {

		public BBParameter<GameObject> attackTelegraph;
		protected override void OnExecute() {
			GameObject.Instantiate(attackTelegraph.value, agent.transform); 
			EndAction(true);
		}
	}
}