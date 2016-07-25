using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BloodSwitch : MonoBehaviour {

  public Attachable attachable;

  public float onThreshold = 1;

  public UnityEvent OnTurnOn;
  public UnityEvent OnTurnOff;

  void Start() {
    if (attachable == null)
      attachable = GetComponent<Attachable>();

    attachable.OnBloodChange.AddListener(OnBloodChange);
  }

  void OnBloodChange(float before, float after) {
    if (before < onThreshold && after >= onThreshold)
      OnTurnOn.Invoke();
    if (before >= onThreshold && after < onThreshold)
      OnTurnOff.Invoke();
  }

}