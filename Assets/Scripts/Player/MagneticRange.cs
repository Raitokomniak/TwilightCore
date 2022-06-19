using UnityEngine;
using System.Collections;

public class MagneticRange : MonoBehaviour {
    AnimationController animationC;
	SpriteRenderer spriteRenderer;
	CircleCollider2D _collider;

    void Awake(){
        animationC = GetComponent<AnimationController>();
    }
	public void Scale(int dir){
		spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<CircleCollider2D>();

		if (dir > 0)
			spriteRenderer.enabled = true;

		else spriteRenderer.enabled = false;

		if (dir > 0 && _collider.radius < 6 || dir < 0 && _collider.radius > 1)
			_collider.radius += dir * 0.2f;
	}

    public void Toggle(bool toggle){
        if(toggle){
            Scale (1);
			animationC.StartRotating(4f);
        }
        else {
            Scale (-1);
			animationC.StopRotating();
        }
    }
}
