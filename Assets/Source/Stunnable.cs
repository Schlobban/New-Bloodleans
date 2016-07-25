using UnityEngine;
using System.Collections;

public class Stunnable : MonoBehaviour {

	public Behaviour[] controlledBehaviour;

	void Stun(bool stunned) {
		foreach (Behaviour b in controlledBehaviour) {
			b.enabled = !stunned;
		}
	}

}
