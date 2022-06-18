using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_MusicalNotes : Pattern
{
    //SPAWNS BULLETS EVENLY IN A CIRCLE AROUND THE SHOOTER

    public P_MusicalNotes(int _bulletCount){
        bulletCount = _bulletCount;
        tempMagnitude = originMagnitude;
    }

    public P_MusicalNotes(){
        bulletCount = 7;
        tempMagnitude = originMagnitude;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        List<string[]> notesprites = new List<string[]>();
        string effect = "";
        string color = "";

        for(int i = 0; i < 7; i++){
            if(i == 0) color = "Blue";
            if(i == 1) color = "Turquoise";
            if(i == 2) color = "Green";
            if(i == 3) color = "Yellow";
            if(i == 4) color = "Red";
            if(i == 5) color = "Purple";
            if(i == 5) color = "Lilac";

            for(int j = 0; j < 3; j++){
                if(j == 0) effect = "NoStem";
                if(j == 1) effect = "1Stem";
                if(j == 2) effect = "2Stem";

                notesprites.Add(new string[3]{"Note", effect, color});
            } 
        }

        notesprites.Add(new string[3]{"Note", "NoStem", "Blue"});
        
        
        Game.control.sound.PlaySound ("Enemy", "Shoot", true);
		
        for (int i = 0; i < bulletCount; i++) {
            
			spawnPosition = SpawnInCircle (pos, 0f, GetAng (i, 360));
			//SpawnBullet (enemyBullet, bulletMovement);
				SpawnBullet (BMP);
            int randomSprite = Random.Range(0,21);
            SetSprite(notesprites[randomSprite][0], notesprites[randomSprite][1], notesprites[randomSprite][2], "Tiny");
            SetGlowSprite(notesprites[randomSprite][0], notesprites[randomSprite][2]);
            BMP.SetSpriteRotation(new Vector3(0,0,0));
		}
    }
}
