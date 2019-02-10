using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class BbaControl : MonoBehaviour 
{

  // --- InputManagerから書き換えられる入力の値 ---
  // --- -1.0 ~ 1.0 の間の値が入る ---
    [SerializeField]
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

  // --- BBAの状態パラメータ ---
  [SerializeField]
  private float hp = 100;
  [SerializeField]
  private bool dash = false;
  private bool drift = false;
  private bool isGoal = false;

  PostProcessingBehaviour behaviour;
  

  // --- ドリフト＆加速周りの変数 ---
  private float defaultMaxSpeed;
  private float defaultAccelPower;
  private float driftPoint = 0.0f;

  // --- 現在の状態パラメータ ---
  private float velocity = 0.0f;
  public float Velocity { get{ return velocity; } }

  // --- 子オブジェクト ---
  private Transform child;

  private GameObject fireEffect;
  private GameObject smokeEffect;

  private GameObject goalText;
  private GameObject gameoverText;

  private Transform mainCamera;
  private float gyroZ;
  public float GyroZ { set { gyroZ = value;} }
  private float accelCamera = 0.1f;


// ----- not used -----
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

    if (transform.position.x > 10.0f)
    {
      transform.position = new Vector3 (10.0f, transform.position.y, transform.position.z);
    }

    if (transform.position.x < -10.0f)
    {
      transform.position = new Vector3 (-10.0f, transform.position.y, transform.position.z);
    }

  }

  /* ---------------------------------------- Steering
   * 左右旋回 : 入力された値をもとにBBAを旋回させる
   */
  private void Steering ()
  {
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
      if (inputAccel > 0.0f)
        velocity -= accelPower * kBrake;
        driftPoint += 1.0f;

      maxSpeed = defaultMaxSpeed;
      accelPower = defaultAccelPower;

      dash = false;
      drift = true;
    }
    else // --- 通常走行 --- 
    {
      trail.SetActive(false);
      driftPoint -= 0.7f;
      if (driftPoint < 0.0f)
        driftPoint = 0.0f;

      if (inputAccel < 0.7f)
        driftPoint = 0.0f;

      if (driftPoint > 0.0f)
        dash = true;
      else
        dash = false;
      
      maxSpeed = defaultMaxSpeed + (driftPoint * 0.3f);
      accelPower = defaultAccelPower + (driftPoint * 0.2f);

      drift = false;
    }
    
    child.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + kRotate, 0.0f);
  } 

  /* ---------------------------------------- CameraManage
   * カメラ管理 : 
   */
  private void CameraManage ()
  {

    if (!isGoal)
    {
      float cameraZ = mainCamera.transform.localPosition.z;

      if (dash)
      {      
        cameraZ -= accelCamera;
        accelCamera *= 1.5f;
        if (cameraZ < -8.5f)
        {
          cameraZ = -8.5f;
          accelCamera = 0.1f;
        }
      }
      else
      {
        cameraZ += accelCamera;
        accelCamera *= 1.5f;
        if (cameraZ > -7.5f)
        {
          cameraZ = -7.5f;
          accelCamera = 0.1f;
        }
      }

      mainCamera.transform.localPosition = new Vector3 (mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y, cameraZ);

      Vector3 nowRot = transform.rotation.eulerAngles;
      mainCamera.rotation = Quaternion.Euler(nowRot.x, nowRot.y, gyroZ);
      goalText.SetActive (false);
    }
    else
    {
      // --- ゴールした時のカメラワーク ---
      mainCamera.transform.LookAt (this.transform.position + new Vector3(0, 2, 0));
      mainCamera.transform.localPosition = Vector3.Slerp(mainCamera.transform.localPosition, new Vector3 (-1.5f, 1.5f, 4), 2.0f * Time.deltaTime);
      velocity = maxSpeed;
      Vector3 nowRot = transform.rotation.eulerAngles;
      goalText.SetActive (true);
      hp = 100.0f;
      //mainCamera.rotation = Quaternion.Euler(nowRot.x, nowRot.y, 0.0f);
    }
  }

  /* ---------------------------------------- HPManage
   * HP管理 : 
   */
  private void HPManage ()
  {
    hp -= 3.0f * Time.deltaTime;
    var setting = behaviour.profile.colorGrading.settings;
    setting.basic.saturation = hp * 0.02f;
    behaviour.profile.colorGrading.settings = setting;
    
    gameoverText.SetActive (false);

    if (hp < 0.0f)
    {
      gameoverText.SetActive (true);
      hp -= 10.0f * Time.deltaTime;
      setting.basic.saturation = hp * 0.02f;
      behaviour.profile.colorGrading.settings = setting;
      velocity = 0.0f;
      Invoke ("SceneReload", 5.0f);
    }
  }

  /* ---------------------------------------- SceneReload
   * シーンの再読み込み : 
   */
  private void SceneReload ()
  {
    SceneManager.LoadScene("Run");
  }

  /* ----------------------------------------- EffectManage
   * エフェクト管理 : 状況に応じてエフェクトの使い分け
   */
  private void EffectManage ()
  {
    if (dash)
      fireEffect.SetActive (true);
    else
      fireEffect.SetActive (false);

    if (drift)
      smokeEffect.SetActive (true);
    else
      smokeEffect.SetActive (false);
  }

  /* ----------------------------------------- OnTriggerEnter
   * 当たり判定
   */
  private void OnTriggerEnter (Collider col)
  {
    if (col.gameObject.tag== "BlueBerry")
    {
      hp += 30.0f;
      if (hp > 100.0f)
        hp = 100.0f;

      Destroy (col.gameObject);
    }

    if (col.gameObject.tag == "ObstacleBig")
    {
      velocity -= 20.0f;
      if (velocity < 0.0f)
        velocity = 0.0f;

      if (dash)
      {
        col.gameObject.AddComponent<Rigidbody>().velocity = new Vector3(0, 30, 0);
      }
    }

    if (col.gameObject.tag == "ObstacleSmall")
    {
      velocity -= 10.0f;
      if (velocity < 0.0f)
        velocity = 0.0f;

      if (dash)
      {
        col.gameObject.AddComponent<Rigidbody>().velocity = new Vector3(0, 30, 0);
      }
    }

    if (col.gameObject.tag == "Goal")
    {
      isGoal = true;
    }

  }



	// Use this for initialization
	void Start () 
  {
		child = transform.GetChild(0);
    mainCamera = GameObject.FindWithTag ("MainCamera").transform;
    trail = GameObject.Find("Trail");
    fireEffect = GameObject.Find ("Fire");
    smokeEffect = GameObject.Find ("Smoke");
    goalText = GameObject.Find ("GoalText");
    gameoverText = GameObject.Find ("GameOverText");
    trailMaterial = trail.GetComponent<Material> ();
    defaultMaxSpeed = maxSpeed;
    defaultAccelPower = accelPower;
    gyroZ = 0.0f;

    // --- PostProcessing ---
    behaviour = mainCamera.GetComponent<PostProcessingBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () 
  {
		MoveForwardAndBackword ();

    if (velocity != 0)
    {
      Steering ();
      if (velocity > 0.0f)
        DriftManage ();
    }
    Resistance ();

    HPManage ();

    EffectManage ();

    CameraManage ();

	}
}
