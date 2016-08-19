using UnityEngine;
using UnityEngine.Events;

public class Attachable : MonoBehaviour {

  public int priority = 1;
  public float blood;
  public float maxBlood;

  public float transferDurationPerUnit = 0.2f;
  public float baseTransferDuration = 0.5f;

  public bool allowInject = true;
  public bool allowWithdraw = true;

  public UnityEvent OnAttached;
  public UnityEvent OnDettached;


  public bool isAttached {
    get { return player != null; }
  }

  private Player player;

  [System.Serializable]
  public class BloodChangeEvent : UnityEvent<float, float> {}

  public BloodChangeEvent OnBloodChange;


  void OnDisable() {
    if (player != null)
      player.SetAttached(null);
  }

  public void Attach(Player player) {
    OnAttached.Invoke();
    this.player = player;
  }

  public void Dettach() {
    OnDettached.Invoke();
    player = null;
  }

  public float TransferBlood(float diff) {
    if (!enabled || (diff < 0 && !allowWithdraw) || (diff > 0 && !allowInject))
      return 0;

    float prev = blood;
    if (blood + diff > maxBlood) {
      blood = maxBlood;
      OnBloodChange.Invoke(prev, blood);
      return maxBlood - prev;
    }
    if (blood + diff < 0) {
      blood = 0;
      OnBloodChange.Invoke(prev, blood);
      return -prev;
    }
    blood += diff;
    OnBloodChange.Invoke(prev, blood);
    return diff;
  }
}