using UnityEngine;
using System.Collections;

public class PlayerSpecialAttack : MonoBehaviour {
	public bool specialAttack;
	public float specialAttackTime;

	public GameObject nightSpecial;
	public GameObject daySpecial;

	public GameObject nightAnimatedSprite;
	public GameObject dayAnimatedSprite;

	GameObject starLightBomb;


	void Awake(){
		starLightBomb = Resources.Load ("StarLightBomb") as GameObject;
	}

	void Update () {
		if(!specialAttack && Input.GetKeyDown(KeyCode.X) && !GameController.gameControl.dialog.handlingDialog)
		{
			if (!GetComponent<PlayerMovement>().focusMode && GameController.gameControl.stats.dayCorePoints >= 20) {
				StartCoroutine(SpecialAttack ("Day"));
			} else if (GetComponent<PlayerMovement>().focusMode && GameController.gameControl.stats.nightCorePoints >= 20) {
				StartCoroutine(SpecialAttack ("Night"));
			} 
			else {
				Debug.Log ("not enough points");
			}
		}

	}

	IEnumerator SpecialAttack(string core)
	{
		specialAttackTime = 4f;
		specialAttack = true;

		GameController.gameControl.stats.DepleteCore (true);

		if (core == "Day") {
			GameController.gameControl.ui.ShowActivatedPhase ("Player", "StarLight");
			daySpecial.SetActive (true);
			dayAnimatedSprite.GetComponent<AnimationController> ().Scale (1, .5f, true, false);
			dayAnimatedSprite.GetComponent<AnimationController> ().rotating = true;
			yield return new WaitForSeconds (1.5f);
			daySpecial.SetActive (false);
			ArrayList bombs = new ArrayList ();

			for (int i = 0; i < 10; i++) {
				GameObject bomb = (GameObject)Instantiate (starLightBomb, new Vector3(-6 + Random.Range(-12, 12), 0 + Random.Range(-10, 10), 0), transform.rotation);
				bomb.SetActive (true);
				bomb.GetComponent<AnimationController> ().rotating = true;
				yield return new WaitForSeconds (.1f);
				bomb.GetComponent<AnimationController> ().Scale (1, 3, true, false);
				bombs.Add (bomb);
			}
			yield return new WaitForSeconds (specialAttackTime);

			foreach (GameObject bomb in bombs) {
				bomb.GetComponent<AnimationController> ().Scale (-1, 3, false, false);
				yield return new WaitForSeconds (.1f);
				Destroy (bomb);
			}
			GameController.gameControl.enemySpawner.DestroyAllProjectiles();
			daySpecial.SetActive (false);

		
		} else if(core == "Night") {
			GameController.gameControl.ui.ShowActivatedPhase ("Player", "Trick or Treat");

			nightSpecial.SetActive (true);
			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (1, 2.5f, true, true);

			yield return new WaitForSeconds (specialAttackTime);

			nightAnimatedSprite.GetComponent<AnimationController> ().Scale (-1, 2.5f, true, true);
			nightSpecial.SetActive (false);
		}

		specialAttack = false;
		//GameController.gameControl.enemySpawner.DestroyAllEnemies();
		//
	}
}
