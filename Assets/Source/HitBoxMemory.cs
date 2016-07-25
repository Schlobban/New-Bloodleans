using UnityEngine;
using System.Collections;

public class HitBoxMemory : MonoBehaviour {

	public LayerMask layers;

	private Collider2D[] hits = new Collider2D[5];
	// Use this for initialization

	void OnTriggerEnter2D(Collider2D col) {
		if (((1 << col.gameObject.layer) & layers.value) == 0)
			return;

		for (int i = 0; i < hits.Length; i++) {
			if (hits[i] == col)
				return;
			if (hits[i] == null) {
				hits[i] = col;
				return;
			}
		}
		Debug.LogError("The array in HitBoxMemory was too small, too many things");
	}
	void OnTriggerExit2D(Collider2D col) {
		if (((1 << col.gameObject.layer) & layers.value) == 0)
			return;

		int last = 0;
		while (last < hits.Length && hits[last] != null)
			last++;
		last--;
		if (last == -1)
			return;

		for (int i = 0; i <= last; i++) {
			if (hits[i] == col) {
				hits[i] = hits[last];
				hits[last] = null;
				return;
			}
		}
	}

	public Attachable GetOverlap() {
		Attachable ret = null;
		foreach (Collider2D hit in hits) {
			if (hit == null)
				break;
			Attachable a = hit.GetComponent<Attachable>();
			if (a != null && (ret == null || a.priority > ret.priority))
				ret = a;
		}
		return ret;
	}

}
