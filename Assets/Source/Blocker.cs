using UnityEngine;

public class Blocker : MonoBehaviour {

  public float speed = 1;

  public Vector3 unblockLocation;
  private Vector3 blockLocation;

  private Vector3 target;

  void Start() {
    blockLocation = transform.position;
    target = blockLocation;
  }

  public void UnBlock() {
    target = unblockLocation;
  }
  public void Block() {
    target = blockLocation;
  }

  void Update() {
    transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
  }

}