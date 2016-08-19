using UnityEngine;
using System.Collections;

public class NBLight : MonoBehaviour {

  public float gradientStart = 5;
  public float gradientEnd = 10;

  private LightOverlay overlay;

  void Start() {
    overlay = Camera.main.GetComponent<LightOverlay>();
    if (enabled)
      OnEnable();
  }

  void OnEnable() {
    if (overlay != null)
      overlay.AddLight(this);
  }
  void OnDisable() {
    if (overlay != null)
      overlay.RemoveLight(this);
  }

}
