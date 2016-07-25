using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class BloodSpriteChange : MonoBehaviour {

	public Attachable attachable;

	private Sprite empty;
	public Sprite[] levels;

	public float levelSize = 1;

	private SpriteRenderer sRenderer;

	void Start() {
		sRenderer = GetComponent<SpriteRenderer>();
		empty = sRenderer.sprite;
		if (attachable == null)
			attachable = GetComponent<Attachable>();
	}

	void Update() {
		if (attachable.blood <= 0) {
			sRenderer.sprite = empty;
		} else {
			int level = Mathf.FloorToInt(attachable.blood / levelSize) - 1;
			sRenderer.sprite = levels[Mathf.Min(Mathf.Max(0, level), levels.Length-1)];
		}
	}

}
