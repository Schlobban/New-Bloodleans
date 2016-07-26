using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class BloodSwitch : MonoBehaviour {

  public Attachable attachable;

  public float lowerOn = 1;
  public float upperOn = 1;

  public UnityEvent OnTurnOn;
  public UnityEvent OnTurnOff;

  void Start() {
    if (attachable == null)
      attachable = GetComponent<Attachable>();

    attachable.OnBloodChange.AddListener(OnBloodChange);
  }

  void OnBloodChange(float before, float after) {
    if (!Inside(before) && Inside(after)) {
      OnTurnOn.Invoke();
    }
    if (Inside(before) && !Inside(after)) {
      OnTurnOff.Invoke();
    }
  }

  bool Inside(float blood) {
    return lowerOn <= blood && blood <= upperOn;
  }

}