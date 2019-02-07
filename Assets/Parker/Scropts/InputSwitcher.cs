using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSwitcher : MonoBehaviour 
{

  private BbaControl bbaControl;

	// Use this for initialization
	void Start () 
  {
		bbaControl = GetComponent<BbaControl> ();
	}
	

#if UNITY_IOS || UNITY_ANDROID
  // Update is called once per frame
	void Update () 
  {
    
	}
#else
	// Update is called once per frame
	void Update () 
  {
		bbaControl.InputAccel = Input.GetAxis ("Vertical");
    bbaControl.InputHundle = Input.GetAxis ("Horizontal");
    Debug.Log (Input.GetAxis ("Horizontal"));
	}
#endif

}
