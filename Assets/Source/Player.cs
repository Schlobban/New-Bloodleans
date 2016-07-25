using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FixedJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour {

	public float speed;
	public float cooldown = 2;

	public float transferWithdraw = 1;
	public float transferInject = 1;
	public float blood = 0;
	public float maxBlood = 3;

	public float health = 3;
	public float maxHealth = 3;

	public HitBoxMemory hitbox;
	public ParticleSystem system;

	private Animator animator;
	private FixedJoint2D joint;
	private Rigidbody2D body;

	private Attachable attached;

	private float cooldownRemaining;

	public GameObject[] destroyOnDeath;

	void Start(){
		animator = GetComponent<Animator>();
		joint = GetComponent<FixedJoint2D>();
		body = GetComponent<Rigidbody2D>();
		//Cursor.visible = false;
	}

	void FixedUpdate() {
		if (health < 0)
			return;
		if (attached == null) {
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			body.AddForce(input * speed);
		}

		animator.SetFloat("Velocity", body.velocity.magnitude);
	}

	void Update() {
		if (health < 0)
			return;

		cooldownRemaining -= Time.deltaTime;
		ParticleSystem.EmissionModule emission = system.emission;
		emission.enabled = cooldownRemaining > 0;

		if (attached == null) {
			if (Input.GetButtonDown("Attach") && cooldownRemaining <= 0) {
				animator.SetTrigger("Stab");
			}

			Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		} else {
			if (Input.GetButtonDown("Dettach")) {
				SetAttached(null);
			}

			if (Input.GetButtonDown("Inject")) {
				blood -= attached.TransferBlood(Mathf.Min(transferInject, blood));
			} else if (Input.GetButtonDown("Withdraw")) {
				blood -= attached.TransferBlood(-Mathf.Min(transferWithdraw, maxBlood - blood));
			}
		}
	}

	public void AlterHealth(float diff) {
		float prevHealth = health;
		health = Mathf.Min(maxHealth, health + diff);
		if (prevHealth >= 0 && health <= 0) {
			animator.SetTrigger("Death");
			foreach (GameObject go in destroyOnDeath) {
				Destroy(go);
			}
		}
	}

	public void DestroySelf() {
		Destroy(gameObject);
	}

	public void CheckStabHit() {
		Attachable hit = hitbox.GetOverlap();
		if (hit == null || !hit.enabled) {
			cooldownRemaining = cooldown;
			return;
		}

		SetAttached(hit);
	}

	public void SetAttached(Attachable attachable) {
		if (attached != null && attachable == null) {
			cooldownRemaining = cooldown;
			attached.Dettach();
			attached = null;
			body.freezeRotation = true;
			animator.SetBool("Attached", false);
			joint.enabled = false;
			return;
		}

		if (attached == null && attachable != null) {
			attached = attachable;
			attachable.Attach(this);
			body.freezeRotation = false;
			animator.SetBool("Attached", true);
			Rigidbody2D otherBody = attachable.GetComponent<Rigidbody2D>();
			if (otherBody == null) {
				otherBody = attachable.GetComponentInParent<Rigidbody2D>();
			}
			joint.enabled = true;
			joint.connectedBody = otherBody;
			return;
		}

		if (attached != null && attachable != null) {
			Debug.LogWarning("Attempting to change the current attachment, not supported atm");
			return;
		}

		// both are null, don't really care
	}

}