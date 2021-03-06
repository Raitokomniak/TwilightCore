﻿using UnityEngine;
using System.Collections;

public class SpriteLibrary : MonoBehaviour {

	public ArrayList allSprites;

	// Use this for initialization
	void Awake () {
		allSprites = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite SetBulletSprite(string shape, string effect, string color){
		string path = "BulletSprites/" + shape + "_" + effect + "_" + color;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}


	public Sprite SetCharacterSprite(string name)
	{
		string path = "CharacterSprites/" + name;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}


}
