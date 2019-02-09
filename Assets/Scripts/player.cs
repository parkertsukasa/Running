using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour {

    float m_speed = 20f;
    float m_HP = 100f;

    public GameObject txt;

    Text m_HPText;

	// Use this for initialization
	void Start () {
        m_HPText = txt.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position += gameObject.transform.forward * m_speed * Time.deltaTime;

        if(Input.GetKey(KeyCode.RightArrow)){
            gameObject.transform.Rotate(new Vector3(0f,3f, 0f));
        }
        if(Input.GetKey(KeyCode.LeftArrow)){
            gameObject.transform.Rotate(new Vector3(0f, -3f, 0f));
        }

        m_HPText.text = m_HP.ToString("f2");
	}

	private void OnTriggerEnter(Collider other)
	{
        if(other.gameObject.tag == "ObstacleSmall"){
            m_HP -= 5.0f;
        }
        if(other.gameObject.tag == "ObstacleBig"){
            m_HP -= 10.0f;
        }
        if(other.gameObject.tag == "BlueBerry"){
            m_HP += 10.0f;
            Destroy(other.gameObject);
        }
	}

	private void OnCollisionStay(Collision collision)
	{
        if(collision.gameObject.tag == "wall"){
            m_HP -= 0.1f;
        }
	}
}
