using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacle : MonoBehaviour {

    public int m_obs_item_Num = 25; //真ん中の細かい障害物の個数分
    public int m_obs_pole_Num = 2; //両はしのポールの障害物の個数
    public int m_obs_end_r_Num = 10; //ベンチとかの個数
    public int m_obs_end_l_Num = 10;
    public int m_obsNum_HighWaySign = 3; //高速の看板の個数
    public int m_blueNum = 1; //ブルーベリーアイの個数

    public float spawn_range = 11.0f; //インスタンスする障害物の横幅の範囲

    Dictionary<int, string> obs_item1 = new Dictionary<int, string>()
    {
        {1, "FuseBox"},{2, "Hidrant"},{3, "MailBox"},{4, "PhoneL"},{5, "PhoneR"}, {6, "PlantPot"}
    };

    Dictionary<int, string> obs_item2 = new Dictionary<int, string>()
    {
        {1, "RoadBlock_A"}, {2, "RoadBlock_B"}, {3, "RoadCone_A"}, {4, "RoadCone_B"}
    };

    Dictionary<int, string> obs_pole = new Dictionary<int, string>()
    {
        {1, "Powerpole_A_A"}, {2, "Powerpole_B_A"},{3, "LampPost_J"}, {4, "LampPost_K"}
    };

    Dictionary<int, string> obs_end_r = new Dictionary<int, string>()
    {
        {1, "Bench_AR"}, {2, "ParkLamp"}
    };

    Dictionary<int, string> obs_end_l = new Dictionary<int, string>()
    {
        {1, "Bench_AL"}, {2, "ParkLamp"}
    };

    Dictionary<int, string> obs_HighWay = new Dictionary<int, string>()
    {
        {1, "HighwaySign_A"}, {2, "HighwaySign_B"}, {3, "HighwaySign_C"}
    };

    // Use this for initialization
    void Start()
    {
        float this_x = gameObject.transform.position.x;
        float this_z = gameObject.transform.position.z;
        float z_first = this_z - 100f;
        float z_range = 200 / m_obs_item_Num;
        string spawn_name;
        int rnd_num;

        GameObject instance; 

        for (int i = 0; i < m_obs_item_Num; i++){
            float pos_x = Random.Range(this_x - spawn_range, this_x + spawn_range);
            float pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_item1.Count + 1);
            spawn_name = obs_item1[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(pos_x, 1.0f, pos_z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;

            pos_x = Random.Range(this_x - spawn_range, this_x + spawn_range);
            pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_item2.Count + 1);
            spawn_name = obs_item2[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(pos_x, 1.0f, pos_z), Quaternion.Euler(-90, 90, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            z_first = z_first + z_range;
        }

        z_first = this_z - 100f;
        z_range = 200 / m_obs_pole_Num;
        for (int i = 0; i < m_obs_pole_Num; i++){
            float pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_pole.Count + 1);
            spawn_name = obs_pole[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(9.5f, 1.0f, pos_z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(-9.5f, 1.0f, pos_z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            z_first = z_first + z_range;
        }

        z_first = this_z - 100f;
        z_range = 200 / m_obs_end_r_Num;
        for (int i = 0; i < m_obs_end_r_Num; i++)
        {
            float pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_end_r.Count + 1);
            spawn_name = obs_end_r[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(7.5f, 1.0f, pos_z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_end_r.Count + 1);
            spawn_name = obs_end_l[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(-7.5f, 1.0f, pos_z), Quaternion.Euler(-90, 180, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            z_first = z_first + z_range;
        }

        z_first = this_z - 100f;
        z_range = 200 / m_obsNum_HighWaySign;
        for (int i = 0; i < m_obsNum_HighWaySign; i++)
        {
            float pos_z = Random.Range(z_first, z_first + z_range);
            rnd_num = Random.Range(1, obs_HighWay.Count + 1);
            spawn_name = obs_HighWay[rnd_num];
            instance = Instantiate(Resources.Load(spawn_name, typeof(GameObject)), new Vector3(0f, 1.0f, pos_z), Quaternion.Euler(-90, 0, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            z_first = z_first + z_range;
        }

        z_first = this_z - 100f;
        z_range = 200 / m_blueNum;
        for (int i = 0; i < m_blueNum; i++)
        {
            float pos_z = Random.Range(z_first, z_first + z_range);
            instance = Instantiate(Resources.Load("BluBluKunV3", typeof(GameObject)), new Vector3(0f, 2.3f, pos_z), Quaternion.Euler(0, 180, 0)) as GameObject;
            instance.transform.parent = gameObject.transform;
            z_first = z_first + z_range;
        }

	}

}
