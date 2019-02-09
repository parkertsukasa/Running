using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireControl : MonoBehaviour 
{

  [SerializeField]
  private bool front;

  private BbaControl bba;

	// Use this for initialization
	void Start () 
  {
		bba = GameObject.Find("BBA").GetComponent<BbaControl>();
	}
	
	// Update is called once per frame
	void Update () 
  {
		if (front)
      transform.localRotation = Quaternion.Euler (90.0f,bba.InputHundle * 35.0f, 90.0f);

    transform.Rotate (Vector3.up * bba.Velocity * -1000);
	}
}
