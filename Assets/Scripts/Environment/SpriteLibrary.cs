using UnityEngine;
using System.Collections;

public class SpriteLibrary : MonoBehaviour {

	public ArrayList allSprites;

	void Awake () {
		allSprites = new ArrayList ();
	}
    
	public Sprite SetBulletSprite(string shape, string effect, string color){
        string path = "";
        if(effect == "" && color == "") 
                path = "Sprites/BulletSprites/" + shape;
		else    path = "Sprites/BulletSprites/" + shape + "_" + effect + "_" + color;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}


    public Color GetColor(string colorName){
        Color color = new Color(0,0,0,0);
        if(colorName == "Red")          color = Color.red;
        else if(colorName == "Orange")       color = new Color(1,0.35f,0,1);
        else if(colorName == "Yellow")       color = Color.yellow;
        else if(colorName == "Green")        color = Color.green;
        else if(colorName == "Turquoise")    color = new Color(0,1,1,1);
        else if(colorName == "Blue")         color = Color.blue;
        else if(colorName == "Lilac")        color = new Color(0.08f,0,1,1);
        else if(colorName == "Purple")       color = new Color(0.5f,0,1,1);
        else if(colorName == "White")        color = Color.white;
        else if(colorName == "Black")        color = Color.black;
        else if(colorName == "BlackLilac")   color = Color.black;
        else if(colorName == "BlackPurple")  color = Color.black;
        else if(colorName != "") Debug.Log("no defined color for " + colorName);
        return color;
    }

	public Sprite SetBulletGlow(string shape){
		string path = "Sprites/BulletSprites/Glow/" + shape;
		Sprite glowSprite = Resources.Load<Sprite> (path);
		return glowSprite;
	}

	public Sprite SetCharacterSprite(string name){
		string path = "Sprites/CharacterSprites/" + name;
		Sprite sprite = Resources.Load<Sprite> (path);
		return sprite;
	}

	public Sprite SetEnemySprite(string name){
		string path = "Sprites/EnemySprites/" + name;
		Sprite sprite = Resources.Load<Sprite>(path);
		return sprite;
	}
}
