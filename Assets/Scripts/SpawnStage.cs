using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStage : MonoBehaviour {
    public float limit_z = 2000f;
	private void OnTriggerEnter(Collider other)
	{
        //ステージのスポーン・通り過ぎたステージのDestroy
        if(other.tag == "Player"){
            Debug.Log("Check");
            Vector3 parent_pos = gameObject.transform.parent.transform.position;
            if(limit_z > parent_pos.z){
                Instantiate(Resources.Load("CellStage", typeof(GameObject)), parent_pos + new Vector3(0f, 0f, 600f), Quaternion.identity); 
            }else if(limit_z <= parent_pos.z && limit_z + 200f > parent_pos.z){
                Instantiate(Resources.Load("Goal", typeof(GameObject)), parent_pos + new Vector3(0f, 0f, 520f), Quaternion.identity); 
            }
            Destroy(gameObject.transform.parent.gameObject, 2);
        }
	}
}
