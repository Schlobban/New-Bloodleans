using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BloodSpriteChange : MonoBehaviour {

	public Attachable attachable;

	private Sprite empty;
	public Sprite[] levels;

	public float levelSize = 1;

	public float animationSpeed;

	private SpriteRenderer sRenderer;

	private float shownAmount;

	void Start() {
		sRenderer = GetComponent<SpriteRenderer>();
		empty = sRenderer.sprite;
		if (attachable == null)
			attachable = GetComponent<Attachable>();
	}

	void Update() {
		if (animationSpeed > 0) {
			shownAmount = Mathf.Lerp(shownAmount, attachable.blood, animationSpeed * Time.deltaTime);
		} else {
			shownAmount = attachable.blood;
		}

		if (shownAmount <= 0) {
			sRenderer.sprite = empty;
		} else {
			int level = Mathf.FloorToInt(shownAmount / levelSize) - 1;
			sRenderer.sprite = levels[Mathf.Min(Mathf.Max(0, level), levels.Length-1)];
		}
	}

}
