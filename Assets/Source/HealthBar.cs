using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public Player player;
	public float adjustSpeed = 4;

	public Color increaseColor;
	public Color decreaseColor;

	private Color defaultColor;
	private Image image;

	private float prevHealth;

	void Start() {
		image = GetComponent<Image>();
		defaultColor = image.color;
		prevHealth = player.health;
	}

	void Update() {
		if (prevHealth != player.health) {
			if (prevHealth < player.health)
				image.color = increaseColor;
			else if (prevHealth > player.health)
				image.color = decreaseColor;
			prevHealth = player.health;
			Vector3 localScale = transform.localScale;
			localScale.y = player.health/player.maxHealth;
			transform.localScale = localScale;
		} else {
			image.color = Color.Lerp(image.color, defaultColor, adjustSpeed * Time.deltaTime);
		}
	}

}
