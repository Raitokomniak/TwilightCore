using UnityEngine;
using System.Collections;

public class MagneticRange : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	CircleCollider2D _collider;

	public void Scale(int dir){
		spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<CircleCollider2D>();

		if (dir > 0)
			spriteRenderer.enabled = true;

		else spriteRenderer.enabled = false;

		if (dir > 0 && _collider.radius < 6 || dir < 0 && _collider.radius > 1)
			_collider.radius += dir * 0.2f;
	}
}
