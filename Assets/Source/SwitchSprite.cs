using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SwitchSprite : MonoBehaviour {

	private Sprite offSprite;
	public Sprite onSprite;

	private SpriteRenderer rend;

	void Start() {
		rend = GetComponent<SpriteRenderer>();
		offSprite = rend.sprite;
	}

	public void TurnOn() {
		rend.sprite = onSprite;
	}
	public void TurnOff() {
		rend.sprite = offSprite;
	}

}