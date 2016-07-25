using UnityEngine;

public class Door : MonoBehaviour {

  private Animator animator;
  private Collider2D collider;

  void Start() {
    animator = GetComponent<Animator>();
    collider = GetComponent<Collider2D>();
  }

  public void Open() {
    animator.SetBool("Open", true);
  }

  public void Close() {
    animator.SetBool("Open", false);
  }

  public void UpdateCollider() {
    collider.enabled = !animator.GetBool("Open");
  }

}