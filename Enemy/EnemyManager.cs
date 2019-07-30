using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Network;
using Assets.Script.Common;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] bornPoints;
    [SerializeField] private GameObject longRangePrefab;
    [SerializeField] private GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //InitializeEnemies(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeEnemies(int round)
    {
        MsgCSAskForEnemies msgAsk = new MsgCSAskForEnemies(round);
        byte[] msgPacked = msgAsk.Marshal();
        SocketClient.netStream.Write(msgPacked, 0, msgPacked.Length);

        byte[] dataReceivedNoHead = SocketClient.RemoveDataHead();
        MsgSCEnemyInitialize msgEnemyData = (MsgSCEnemyInitialize)
            (new UnifromUnmarshal().Unmarshal(dataReceivedNoHead));

        int enemyNum = msgEnemyData.enemyNum;
        for (int i = 0;i < enemyNum;++i)
        {
            int enemyID = msgEnemyData.enemies[i].enemyID;
            int enemyType = msgEnemyData.enemies[i].enemyType;
            GameObject currEnemy;
            Debug.Log("enemy num: " + enemyNum);
            Debug.Log("enemy id: " + enemyID);
            if (enemyType == 0)//实例化一个远程敌人
                currEnemy = GameObject.Instantiate(
                    longRangePrefab,
                    bornPoints[enemyID].position,
                    bornPoints[enemyID].rotation);
            else//实例化一个爆炸敌人
                currEnemy = GameObject.Instantiate(
                        explodePrefab,
                        bornPoints[enemyID].position,
                        bornPoints[enemyID].rotation);

            currEnemy.GetComponent<EnemyController>().id = enemyID;
            //{
            //    //实例化一个远程敌人
            //    GameObject longRangeEnemy = GameObject.Instantiate(
            //        longRangePrefab,
            //        bornPoints[enemyID].position,
            //        bornPoints[enemyID].rotation);
            //    longRangeEnemy.GetComponent<EnemyController>().id = enemyID;
            //}
            //else
            //{

            //}
        }

    }
}
