using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class FloorSwitch : MonoBehaviour {

	public Sprite onSprite;
	private Sprite offSprite;

	public UnityEvent OnTurnOn;
	public UnityEvent OnTurnOff;

	private SpriteRenderer rend;

	private int unitCount;

	void Start() {
		rend = GetComponent<SpriteRenderer>();
		offSprite = rend.sprite;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		unitCount++;
		if (unitCount == 1) {
			rend.sprite = onSprite;
			OnTurnOn.Invoke();
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		unitCount--;
		if (unitCount == 0) {
			rend.sprite = offSprite;
			OnTurnOff.Invoke();
		}
	}
}