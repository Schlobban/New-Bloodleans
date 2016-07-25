using UnityEngine;
using System.Collections;

public class SyringeBlood : MonoBehaviour {

	public Player player;
	private float scale;
	public float threshold;
	public float speed;

	void Update () {
		float targetScale = player.blood >= threshold ? 1 : 0;
		scale = Mathf.Lerp (scale, targetScale, speed * Time.deltaTime);
		Vector3 localScale = transform.localScale;
		localScale.y = scale;
		transform.localScale = localScale;
	}

}
