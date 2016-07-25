using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RedTintWhenBLoody : MonoBehaviour {

  public Attachable attachable;

  private SpriteRenderer rend;

  void Start() {
    rend = GetComponent<SpriteRenderer>();
    if (attachable == null)
      attachable = GetComponent<Attachable>();
  }

  void Update() {
    if (attachable.blood <= 0) {
      rend.color = Color.white;
    } else {
      rend.color = Color.Lerp(Color.white, Color.red, attachable.blood / attachable.maxBlood);
    }
  }

}