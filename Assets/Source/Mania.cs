using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Mania : MonoBehaviour {

	public float speed;
	public float turnSpeed;
	private Transform target;
	public float aggroDistance;

	public float advanceAngle = 90;
	public float rotateAngle = 40;

	private Rigidbody2D body;
	private Animator animator;

	public Attachable attachable;

	void Start () {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
			if (go.GetComponent<Player>() != null) {
				target = go.transform;
				break;
			}
		}
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		if (attachable == null)
			attachable = GetComponent<Attachable>();
	}

	void FixedUpdate() {
		animator.SetFloat("Velocity", body.velocity.magnitude);
		if (attachable.blood <= 0) {
			animator.SetBool("Active", false);
			return;
		}


		Vector2 dir = target.position - transform.position;
		if (dir.sqrMagnitude > aggroDistance * aggroDistance) {
			animator.SetBool("Active", false);
			return;
		}

		animator.SetBool("Active", true);

		if (attachable.isAttached)
			return;

		float angle = Vector2.Angle(transform.right, dir);
		if (angle > rotateAngle / 2) {
			body.AddTorque(Mathf.Sign(Vector3.Cross(transform.right, dir).z) * turnSpeed);
		}

		if (angle < advanceAngle / 2) {
			float dist = dir.magnitude;
			dir /= dist;

			if (dist < aggroDistance) {
				body.AddForce(dir * speed);
			}
		}
	}

}
