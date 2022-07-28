using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class ShapeVectorSaveData {
    public List<Vector3> shapeVectors;
    public ShapeVectorSaveData(){

    }
}
public class SaveShapeVectors : MonoBehaviour
{
    
    
    void Awake(){
        ShapeVectorSaveData data = new ShapeVectorSaveData();
        data.shapeVectors = new List<Vector3>();
        for(int i = 0; i < transform.childCount; i++){
            data.shapeVectors.Add(transform.GetChild(i).position);
        }
        Debug.Log(data.shapeVectors.Count);
        string dataString = JsonUtility.ToJson(data);

        File.WriteAllText(Application.dataPath + "/Resources/PatternShapeData/shape_" + gameObject.name + ".json", dataString);
    }

}
