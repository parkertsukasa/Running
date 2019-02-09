using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreater : MonoBehaviour
{
  [SerializeField]
  private int roadLength;

  public GameObject[] walls;
  
  public GameObject road;

  public GameObject obstacles;


  // Start is called before the first frame update
  void Start()
  {
    for (int i = 0; i < 1000; i++)
    {
      int j = Random.Range (0, 12);
      int k = Random.Range (0, 12);
      int l = Random.Range (0,2);
      Quaternion rot = Quaternion.Euler (0.0f, (float)l * 180.0f, 0.0f);
      Instantiate (walls[j], new Vector3 (-20, 1, i * 6), rot);
      Instantiate (walls[k], new Vector3 (20, 1, i * 6), rot);

      if (i%10 == 0)
        Instantiate (road, new Vector3 (0, -9.5f, i * 6), Quaternion.Euler (-90, 0, 0));

      if (i%200 == 0)
        Instantiate (obstacles, new Vector3 (0, 0, i), Quaternion.identity);
    }
  }

  // Update is called once per frame
  void Update()
  {
    
  }
}
