using UnityEngine;
using UnityEngine.Events;

public class MultiSwitch : MonoBehaviour {

	public int threshold = 2;

	public UnityEvent OnTurnOn;
	public UnityEvent OnTurnOff;

	private int currentCharge;

	public void ChargeUp() {
		currentCharge++;
		if (currentCharge == threshold)
			OnTurnOn.Invoke();
	}

	public void ChargeDown() {
		currentCharge--;
		if (currentCharge == threshold-1)
			OnTurnOff.Invoke();
	}

}
