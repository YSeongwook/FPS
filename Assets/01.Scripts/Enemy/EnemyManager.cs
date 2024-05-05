using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int EnemyNum = 0;
    public GameObject EnemyPrfab;
    public List<Transform> NorthWayPoints;
    public List<Transform> SouthWayPoints;
    public List<Transform> EastWayPoints;
    public List<Transform> WestWayPoints;
    public List<Transform> CenterWayPoints;

    public Vector3 enemyposition;
    public Quaternion enemyrotation;

    private int spawnIdx = 0;

    //public void Start()
    //{
    //    var group = GameObject.Find("WayPointGroup");

    //    group.GetComponentsInChildren<Transform>(wayPoints);
    //    // 첫번째 요소엔 부모의 transform이 들어가기 때문 -> waypointgroup의 transform이 들어감, point들만 남게 
    //    wayPoints.RemoveAt(0);

    //    for (int i = 0; i < EnemyNum; i++)
    //    {
    //        spawnIdx = Random.Range(0, wayPoints.Count);
    //        enemyposition = wayPoints[spawnIdx].position;
    //        enemyrotation = Quaternion.identity;
    //        GameObject enemy = Instantiate(EnemyPrfab, enemyposition, enemyrotation);
    //    }

    //}

    public void Start()
    {
        var NorthGroup = GameObject.Find("Spawn North");
        var SouthGroup = GameObject.Find("Spawn South");
        var EastGroup = GameObject.Find("Spawn East");
        var WestGroup = GameObject.Find("Spawn West");
        var CenterGroup = GameObject.Find("Spawn Center");

        NorthGroup.GetComponentsInChildren<Transform>(NorthWayPoints);
        SouthGroup.GetComponentsInChildren<Transform>(SouthWayPoints);
        EastGroup.GetComponentsInChildren<Transform>(EastWayPoints);
        WestGroup.GetComponentsInChildren<Transform>(WestWayPoints);
        CenterGroup.GetComponentsInChildren<Transform>(CenterWayPoints);


        for (int i = 0; i < EnemyNum / 5; i++)
            RandomSpwan(NorthWayPoints);
        for (int i = 0; i < EnemyNum / 5; i++)
            RandomSpwan(SouthWayPoints);
        for (int i = 0; i < EnemyNum / 5; i++)
            RandomSpwan(EastWayPoints);
        for (int i = 0; i < EnemyNum / 5; i++)
            RandomSpwan(WestWayPoints);
        for (int i = 0; i < EnemyNum - EnemyNum / 5 * 4; i++)
            RandomSpwan(CenterWayPoints);
    }


    public void RandomSpwan(List<Transform> wayPoints)
    {
        // 첫번째 요소엔 부모의 transform이 들어가기 때문 -> waypointgroup의 transform이 들어감, point들만 남게 
        wayPoints.RemoveAt(0);
        spawnIdx = Random.Range(0, wayPoints.Count);
        Debug.Log(wayPoints[spawnIdx]);
        enemyposition = wayPoints[spawnIdx].position;
        enemyrotation = Quaternion.identity;
        GameObject enemy = Instantiate(EnemyPrfab, enemyposition, enemyrotation);
    }

}
