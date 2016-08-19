using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FixedJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : MonoBehaviour {

	public enum Attack {
		NONE, WITHDRAW, INJECT
	}

	public float speed;
	public float speedWhileCharging;

	public float transferWithdraw = 1;
	public float transferInject = 1;
	public float blood = 0;
	public float maxBlood = 3;

	public float health = 3;
	public float maxHealth = 3;

	public HitBoxMemory hitbox;

	private Animator animator;
	private FixedJoint2D joint;
	private Rigidbody2D body;

	private Attachable attached;

	public GameObject[] destroyOnDeath;

	private Attack currentAttack;
	private float attachRemaining;

	void Start() {
		animator = GetComponent<Animator>();
		joint = GetComponent<FixedJoint2D>();
		body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		if (health < 0)
			return;
		if (attached == null) {
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			body.AddForce(input * (currentAttack == Attack.NONE ? speed : speedWhileCharging));
		}

		animator.SetFloat("Velocity", body.velocity.magnitude);
	}

	void Update() {
		if (health < 0)
			return;

		if (currentAttack == Attack.NONE) {
			if (Input.GetButtonDown("WithdrawStab")) {
				animator.SetBool("Charging", true);
				currentAttack = Attack.WITHDRAW;
			} else if (Input.GetButtonDown("InjectStab")) {
				animator.SetBool("Charging", true);
				currentAttack = Attack.INJECT;
			}
		}
		if (currentAttack != Attack.NONE) {
			if (currentAttack == Attack.WITHDRAW && Input.GetButtonUp("WithdrawStab")) {
				if (animator.GetCurrentAnimatorStateInfo(1).IsName("PlayerBodyCharge")) {
					currentAttack = Attack.NONE;
				}
				animator.SetBool("Charging", false);
			} else if (currentAttack == Attack.INJECT && Input.GetButtonUp("InjectStab")) {
				if (animator.GetCurrentAnimatorStateInfo(1).IsName("PlayerBodyCharge")) {
					currentAttack = Attack.NONE;
				}
				animator.SetBool("Charging", false);
			}
		}

		if (attached == null) {
			Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
			float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		} else {
			attachRemaining -= Time.deltaTime;
			if (attachRemaining <= 0) {
				SetAttached(null);
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

	public void StabOver() {
		currentAttack = Attack.NONE;
	}

	public void CheckStabHit() {
		Attachable hit = hitbox.GetOverlap();

		SetAttached(hit);
	}

	public void SetAttached(Attachable attachable) {
		if (attached != null && attachable == null) {
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

			float transferAmount = 0;
			if (currentAttack == Attack.WITHDRAW) {
				transferAmount = attached.TransferBlood(-Mathf.Min(transferWithdraw, maxBlood - blood));
			}
			if (currentAttack == Attack.INJECT) {
				transferAmount = attached.TransferBlood(Mathf.Min(transferInject, blood));
			}
			blood -= transferAmount;
			attachRemaining = attached.baseTransferDuration + Mathf.Abs(transferAmount) * attached.transferDurationPerUnit;
			return;
		}

		if (attached != null && attachable != null) {
			Debug.LogWarning("Attempting to change the current attachment, not supported atm");
			return;
		}

		// both are null, don't really care
	}

}