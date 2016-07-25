using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	void Start () {
    if (target == null) {
      foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
        if (go.GetComponent<Player>() != null) {
          target = go.transform;
          break;
        }
      }
    }
	}

	void LateUpdate () {
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}
}
