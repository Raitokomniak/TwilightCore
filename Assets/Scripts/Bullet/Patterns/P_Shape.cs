using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Shape : Pattern
{
      //SPAWNS BULLETS IN A SHAPE

    public P_Shape(int _bulletCount, string _shapeName, int _shapeSize){
        shapeName = _shapeName;
        shapeSize = _shapeSize;
        bulletCount = _bulletCount;
        tempMagnitude = originMagnitude;
    }

    public P_Shape(){
        bulletCount = 7;
        tempMagnitude = originMagnitude;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;
        
        Game.control.sound.PlaySound ("Enemy", "Shoot", true);
        float startRot = startingRotation;
        while (!stop) {
            

            if(shapeName == "Circle"){
                for (int i = 0; i < bulletCount; i++) {
                    spawnPosition = SpawnInCircle (pos, 0f, GetAng (i + 20, 380, bulletCount));
                    SpawnBullet ();
                }
                allBulletsSpawned = true;
            }
            if(shapeName == "Ring"){
                SetSprite("Coin", "Small");
                for (int i = 0; i < bulletCount; i++) {
                    spawnPosition = SpawnInCircle (pos, .75f * shapeSize, GetAng (i, 360, bulletCount));
                    SpawnBullet ();
                }
                int BC = bulletCount / 2;
                for (int i = 0; i < BC; i++) {
                    spawnPosition = SpawnInCircle (pos, .5f * shapeSize, GetAng (i, 360, BC));
                    SpawnBullet ();
                }
                SetSprite("Jewel", "Glow", "Red", "Big");
                spawnPosition = SpawnInCircle (pos, -.75f * shapeSize, GetAng (0, 360, bulletCount));
                SpawnBullet ();
                allBulletsSpawned = true;
            }
            if(shapeName == "Earring"){
                
                SetSprite("Coin", "Small");
                for (int i = 0; i < bulletCount ; i++) {
                    spawnPosition = SpawnInCircle (pos, .75f * shapeSize, GetAng (i, 360, bulletCount));
                    

                    SpawnBullet ();
                }
                int BC = bulletCount / 2;
                for (int i = 0; i < BC; i++) {
                    spawnPosition = SpawnInCircle (pos, .25f * shapeSize, GetAng (i, 360, BC));
                    if(i == BC / 2)  SetSprite("Jewel", "Glow", "Green", "Medium");
                    else SetSprite("Coin", "Small");
                    SpawnBullet ();
                }
                allBulletsSpawned = true;
            }
            if(shapeName == "TwoCircles"){
                
                for (int i = 0; i < bulletCount ; i++) {
                    spawnPosition = SpawnInCircle (pos, .75f * shapeSize, GetAng (i, 360, bulletCount));
                    SpawnBullet ();
                }
                int BC = bulletCount / 2;
                for (int i = 0; i < BC; i++) {
                    spawnPosition = SpawnInCircle (pos, .25f * shapeSize, GetAng (i, 360, BC));
                    SpawnBullet ();
                }
                allBulletsSpawned = true;
            }
            if(shapeName == "ThreeCircles"){
                
                for (int i = 0; i < bulletCount ; i++) {
                    spawnPosition = SpawnInCircle (pos, .75f * shapeSize, GetAng (i, 360, bulletCount));
                    SpawnBullet ();
                }
                int BC = bulletCount / 2;
                for (int i = 0; i < BC; i++) {
                    spawnPosition = SpawnInCircle (pos, .25f * shapeSize, GetAng (i, 360, BC));
                    SpawnBullet ();
                }
                BC = bulletCount / 3;
                for (int i = 0; i < BC; i++) {
                    spawnPosition = SpawnInCircle (pos, .25f * shapeSize, GetAng (i, 360, BC));
                    SpawnBullet ();
                }
                allBulletsSpawned = true;
            }
            else if(shapeName == "OvalEye"){
                
                bulletCount = 3;
                float radiusY = 1f;
                float decrease = .3f;

                for (int i = 0; i < bulletCount; i++) {
                    radiusY = radiusY - decrease * i; // decrease * i * decrease;
                    spawnPosition = SpawnInCircle (pos, radiusY, GetAng (i, 90, bulletCount));
                    SpawnBullet ();
                    if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			    }
                radiusY = 2.5f;
                for (int i = 0; i < bulletCount; i++) {
                    radiusY = radiusY - decrease * i; // decrease * i * decrease;
                    spawnPosition = SpawnInCircle (pos, radiusY, GetAng (i, -90, bulletCount));
                    SpawnBullet ();
                    if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			    }
                radiusY = 2.5f;
                for (int i = 0; i < bulletCount; i++) {
                    radiusY = radiusY - decrease * i; // decrease * i * decrease;
                    spawnPosition = SpawnInCircle (pos, radiusY, 180 + GetAng (i, 90, bulletCount));
                    SpawnBullet ();
                    if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			    }
                radiusY = 2.5f;
                for (int i = 0; i < bulletCount; i++) {
                    radiusY = radiusY - decrease * i; // decrease * i * decrease;
                    spawnPosition = SpawnInCircle (pos, radiusY, 180 +  GetAng (i, -90, bulletCount));
                    SpawnBullet ();
                    if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			    }
		    }
        else if(shapeName == "SnowFlake"){
            for(int j = -16; j <= 16; j++){
                    float radiusY = 1 + (j * .1f);
                    int i = j + 16;
                    spawnPosition = SpawnInCircle (pos - new Vector3(2,0,0), radiusY, GetAng (i, 360, bulletCount) + startRot);
                    SpawnBullet ();
            }
        }
        else if(shapeName == "Vidya"){
            List<Vector3> spawnPositions = Game.control.io.LoadShape("vidya");

            for(int i = 0; i < 47; i++){
                spawnPosition = spawnPositions[i];
                bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                SpawnBullet();
                
                spawnPosition = spawnPositions[i + 47];
                bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                SpawnBullet();

                if(i + 94 < spawnPositions.Count) {
                    spawnPosition = spawnPositions[i + 94];
                    bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                    SpawnBullet();
                }

                yield return new WaitForSeconds(0.05f);
            }

            allBulletsSpawned = true;
            SetSprite("Circle", "Glow", "BlackBlue", "Medium"); 
        }
        else if(shapeName == "VidyaDrop"){
            List<Vector3> spawnPositions = Game.control.io.LoadShape("vidya");
            bulletRotation = Quaternion.Euler(0,0,0);
            for(int i = 0; i < 47; i++){
                spawnPosition = spawnPositions[i];
                SpawnBullet();
                
                spawnPosition = spawnPositions[i + 47];
                SpawnBullet();

                if(i + 94 < spawnPositions.Count) {
                    spawnPosition = spawnPositions[i + 94];
                    SpawnBullet();
                }

                yield return new WaitForSeconds(0.05f);
            }

            allBulletsSpawned = true;
        }

        else if(shapeName == "Gayatri"){
            List<Vector3> spawnPositions = Game.control.io.LoadShape("gayatri");

            for(int i = 0; i < 65; i++){
                spawnPosition = spawnPositions[i];
                bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                SpawnBullet();
                
                spawnPosition = spawnPositions[i + 65];
                bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                SpawnBullet();

                if(i + 130 < spawnPositions.Count) {
                    spawnPosition = spawnPositions[i + 130];
                    bulletRotation = Quaternion.Euler(0,0, Random.Range(0,360));
                    SpawnBullet();
                }

                yield return new WaitForSeconds(0.02f);
            }
            
            allBulletsSpawned = true;
            ModifyAllBullets(new BMP_3DRotation(this, false, true));
            SetSprite("Coin", "Huge"); 

            yield return new WaitForSeconds(2f);
            ModifyAllBullets("speed", -4);
        }
        
        if(!infinite) break;
        yield return null;
    }
    }
}
