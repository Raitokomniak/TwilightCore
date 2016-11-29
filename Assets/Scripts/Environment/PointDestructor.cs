using UnityEngine;
using System.Collections;

public class PointDestructor : MonoBehaviour {

	bool onMagneticRange;

	void Awake () {
		onMagneticRange = false;
	}

	void Update () {
		if(transform.position.y <= GameController.gameControl.ui.playAreaBottomWall.transform.position.y){
			Destroy(this.gameObject);
		}
		if (onMagneticRange) {
			Vector3 newPosition = GameController.gameControl.stage.Player.transform.position;
			GetComponent<Rigidbody2D> ().isKinematic = true;
			transform.position = Vector3.LerpUnclamped (transform.position, newPosition, Time.deltaTime * 9f);
		}
	}

	void OnTriggerStay2D(Collider2D c)
	{
		GameObject player = GameController.gameControl.stage.Player;
		if (c.tag == "MagneticRange" && !player.GetComponent<PlayerSpecialAttack>().specialAttack) {
			onMagneticRange = true;
		}

	}

}
