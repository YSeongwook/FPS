using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int EnemyNum = 0;
    public GameObject EnemyPrfab;
    public List<Transform> wayPoints;
    public Vector3 enemyposition;
    public Quaternion enemyrotation;

    private int spawnIdx = 0;
    public void Start()
    {
        var group = GameObject.Find("WayPointGroup");

        group.GetComponentsInChildren<Transform>(wayPoints);
        // 첫번째 요소엔 부모의 transform이 들어가기 때문 -> waypointgroup의 transform이 들어감, point들만 남게 
        wayPoints.RemoveAt(0);

        for (int i =0; i<EnemyNum; i++)
        {
            spawnIdx = Random.Range(0, wayPoints.Count);
            enemyposition = wayPoints[spawnIdx].position;
            enemyrotation = Quaternion.identity;
            GameObject enemy = Instantiate(EnemyPrfab, enemyposition, enemyrotation);
        }

    }
}
