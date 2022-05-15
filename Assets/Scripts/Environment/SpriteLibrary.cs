using UnityEngine;
using System.Collections;

public class SpriteLibrary : MonoBehaviour {

	public ArrayList allSprites;

	void Awake () {
		allSprites = new ArrayList ();
	}
	public Sprite SetBulletSprite(string shape, string effect, string color){
		string path = "Sprites/BulletSprites/" + shape + "_" + effect + "_" + color;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}

	public Sprite SetBulletSprite(string shape){
		string path = "Sprites/BulletSprites/" + shape;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}

	public Sprite SetCharacterSprite(string name){
		string path = "Sprites/CharacterSprites/" + name;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}

	//not used yet
	public Sprite SetEnemySprite(string name){
		string path = "Sprites/EnemySprites/" + name;
		Sprite sprite = Resources.Load<Sprite>(path);
		return sprite;
	}


}
