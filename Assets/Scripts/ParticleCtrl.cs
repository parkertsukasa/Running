using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ParticleCtrl : MonoBehaviour
{
    GameObject pObject;
    ParticleSystem particle;

    public float slowdownFactor = 0.25f;
	public float slowdownLength = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        pObject = GameObject.Find ("Sparks");
        particle = pObject.GetComponent<ParticleSystem> ();
    }

    void Update()
    {
        //パーティクルの生成を停止
        if (Input.GetKey(KeyCode.A)){
            particle.Stop();
        }

        //パーティクルの生成スピードを遅くする（演出系で使えそう）
        if (Input.GetKey (KeyCode.Space)) {
			Time.timeScale = slowdownFactor;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		} else {
			Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
			Time.timeScale = Mathf.Clamp (Time.timeScale, 0f, 1f);
		}
    }
}
