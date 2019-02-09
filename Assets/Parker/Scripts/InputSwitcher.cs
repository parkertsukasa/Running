using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSwitcher : MonoBehaviour 
{

  private BbaControl bbaControl;

  [SerializeField]
  private float buttonSensitive = 0.1f;

  [SerializeField]
  private bool accelButtonPushed = false;
  [SerializeField]
  private bool breakeButtonPushed = false;

  private Vector3 defaultGyro;
  private bool defaultSet = false;
  public Text text;

  /* ---------- AccelButtonPushed ----------
   * アクセルボタンが押された時の挙動
   */
  public void AccelButtonPushed ()
  {
    accelButtonPushed = true;

    // --- 最初にアクセルボタンを押すタイミングでジャイロセンサをキャリブレートする ---
    if (!defaultSet)
    {
      defaultGyro = Input.gyro.attitude.eulerAngles;
      defaultSet = true;
    }
  }

  /* ---------- AccelButtonReleased ----------
   * アクセルボタンが離された時の挙動
   */
  public void AccelButtonReleased ()
  {
    accelButtonPushed = false;
  }

  /* ---------- BreakeButtonPushed ----------
   * ブレーキボタンが押された時の挙動
   */
  public void BreakeButtonPushed ()
  {
    breakeButtonPushed = true;
  }

  /* ---------- BreakeButtonReleased ----------
   * ブレーキボタンが離された時の挙動
   */
  public void BreakeButtonReleased ()
  {
    breakeButtonPushed = false;
  }

  /* ---------- IncreaseAccel ----------
   * アクセルを増加させる
   */
  public void IncreaseAccel ()
  {
    float tempInputAccel = bbaControl.InputAccel;
    tempInputAccel += buttonSensitive;
    if (tempInputAccel > 1.0f)
      tempInputAccel = 1.0f;

    bbaControl.InputAccel = tempInputAccel;
  }

  /* ---------- IncreaseAccel ----------
   * アクセルを増加させる
   */
  public void DecreaseAccel ()
  {
    float tempInputAccel = bbaControl.InputAccel;
    tempInputAccel -= buttonSensitive;
    if (tempInputAccel < -1.0f)
      tempInputAccel = -1.0f;

    bbaControl.InputAccel = tempInputAccel;
  }

  /* ---------- ResetZeroAccel ----------
   * アクセルを0に寄せる
   */
  public void TowordZeroAccel ()
  {
    float tempInputAccel = bbaControl.InputAccel;
    if (tempInputAccel > 0.1f)
      tempInputAccel -= 0.1f;
    else if (tempInputAccel < -0.1f)
      tempInputAccel += 0.1f;
    else
      tempInputAccel = 0.0f;

    bbaControl.InputAccel = tempInputAccel;
  }

  /* ---------- ButtonUpdate ----------
   * ボタン押している間の処理
   */
  private void ButtonUpdate ()
  {
    if (accelButtonPushed)
      IncreaseAccel ();

    if (breakeButtonPushed)
      DecreaseAccel ();

    if (!accelButtonPushed && !breakeButtonPushed)
      TowordZeroAccel ();
  }

	// Use this for initialization
	void Start () 
  {
		bbaControl = GetComponent<BbaControl> ();
    Input.gyro.enabled = true; // ジャイロ有効
    defaultGyro = Input.gyro.attitude.eulerAngles;
	}


	private void GetGyroInput ()
  {
    Vector3 inputGyro = Input.gyro.attitude.eulerAngles - defaultGyro;
    
    text.text = inputGyro.ToString();

    float hundle = -inputGyro.z * 0.05f;
    if (hundle > 1.0f)
      hundle = 1.0f;

    if (hundle < -1.0f)
      hundle = -1.0f;

    bbaControl.InputHundle = hundle;

    // --- カメラの角度調整 ---
    if (defaultSet)
      bbaControl.GyroZ = inputGyro.z;
  }

  // Update is called once per frame
	void Update () 
  {
    bbaControl.InputAccel = Input.GetAxis ("Vertical");
    bbaControl.InputHundle = Input.GetAxis ("Horizontal");
    ButtonUpdate ();
    GetGyroInput ();
    bbaControl.InputHundle = Input.GetAxis ("Horizontal");
	}
}
