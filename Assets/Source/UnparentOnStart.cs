using UnityEngine;

public class UnparentOnStart : MonoBehaviour {

  void Start() {
    transform.DetachChildren();
    Destroy(gameObject);
  }

}
