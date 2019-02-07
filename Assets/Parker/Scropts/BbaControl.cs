using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BbaControl : MonoBehaviour 
{

  // --- InputManagerから書き換えられる入力の値 ---
  // --- 0.0 ~ 1.0 の間の値が入る ---
  private float inputAccel = 0.0f;
  public float InputAccel { set{ inputAccel = value; } get{ return inputAccel; } }

  private float inputHundle = 0.0f;
  public float InputHundle { set{ inputHundle = value; } get{ return inputHundle; } }

  private float pastHundle = 0.0f;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

  // --- BBAの性能パラメータ ---
  [SerializeField]
  private float maxSpeed;
  [SerializeField]
  private float accelPower;
  [SerializeField]
  private float hundlePower;

  [SerializeField]
  private float cameraSpeed;

  // --- 現在の状態パラメータ ---
  private float velocity = 0.0f;

  // --- 子オブジェクト ---
  private Transform child;

  private Transform mainCamera;

  private GameObject trail;
  private Material trailMaterial;

  private float cameraPositionY = 3.0f;
  private float cameraPositionZ = -7.5f;

  /* ---------------------------------------- MoveForwardAndBackword
   * 前後移動 : 入力された値をもとにBBAを前後に動かす
   */
  private void MoveForwardAndBackword ()
  {
    velocity += (accelPower * inputAccel * Time.deltaTime);

    if (velocity > maxSpeed * Time.deltaTime)
      velocity = maxSpeed * Time.deltaTime;

    if (velocity < (maxSpeed  * Time.deltaTime * -0.2f))
      velocity = maxSpeed * Time.deltaTime * -0.2f;

    transform.Translate (Vector3.forward * velocity);

  }

  /* ---------------------------------------- Steering
   * 左右旋回 : 入力された値をもとにBBAを旋回させる
   */
  private void Steering ()
  {
    if (velocity != 0.0f)
      transform.Rotate (transform.up * inputHundle * hundlePower);
  }

  /* ---------------------------------------- Resistance
   * 抵抗 : 徐々に速度を遅くしていく
   */
  private void Resistance ()
  {
    if (velocity > 0.1f)
      velocity -= (maxSpeed * 0.01f * Time.deltaTime);
    else if (velocity < -0.1f)
      velocity += (maxSpeed * 0.01f * Time.deltaTime);
    else
      velocity = 0.0f;
  }  

  /* ---------------------------------------- DriftManage
   * ドリフト管理 : 
   */
  void DriftManage ()
  {
    if (Mathf.Abs(inputHundle) < 0.001f)
      inputHundle = 0.0f;

    float kRotate = inputHundle * 45;

    float kBrake = (Mathf.Abs(kRotate) - 20) * 0.0005f;

    // --- ドリフト中 ---
    if (kRotate > 30.0f || kRotate < -30.0f)
    {
      trail.SetActive(true);
      //trailMaterial.color = new Color(0.0f,  0.0f, 0.0f, trailAlpha);

      velocity -= accelPower * kBrake;
    }
    else
    {
      trail.SetActive(false);
    }
    
/*
    // --- 直進している時 ---
    if (inputHundle == 0.0f)
    {
      cameraPositionY += cameraSpeed * 3 * Time.deltaTime;// 2.0
      if (cameraPositionY > 3.0f)
        cameraPositionY = 3.0f;

      cameraPositionZ -= cameraSpeed * 3 * Time.deltaTime;// 4
      if (cameraPositionZ < -7.0f)
        cameraPositionZ = -7.0f;

        
      trail.SetActive(false);
    }
    else// --- ドリフト中 ---
    {
      cameraPositionY -= cameraSpeed * Time.deltaTime;
      if (cameraPositionY < 2.5f)
        cameraPositionY = 2.5f;

      cameraPositionZ += cameraSpeed * Time.deltaTime;
      if (cameraPositionZ > -6.5f)
        cameraPositionZ = -6.5f;

      trail.SetActive(true);
    }

*/

    child.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + kRotate, 0.0f);
    mainCamera.localPosition = new Vector3(0.0f, cameraPositionY, cameraPositionZ);
  } 

	// Use this for initialization
	void Start () 
  {
		child = transform.GetChild(0);
    mainCamera = GameObject.FindWithTag ("MainCamera").transform;
    trail = GameObject.Find("Trail");
    trailMaterial = trail.GetComponent<Material> ();
	}
	
	// Update is called once per frame
	void Update () 
  {
		MoveForwardAndBackword ();
    Steering ();
    Resistance ();
    DriftManage ();
	}
}
