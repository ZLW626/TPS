using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currRound = 1;
    private int countdown = 10;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Text roundNumText;
    [SerializeField] private Text countdownText;
    [SerializeField] private InputManager inputManager;

    private bool tempFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        //enemyManager.InitializeEnemies(currRound);

        StartCoroutine(RoundLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PreRound()
    {
        //UI
        roundNumText.text = "Round " + currRound;
        countdownText.text = "";

        //初始化敌人
        enemyManager.InitializeEnemies(currRound);

        //使玩家待机
        inputManager.enablePlayer = false;

        yield return new WaitForSeconds(3f);
    }

    private IEnumerator InRound()
    {
        //UI
        roundNumText.text = "";

        //激活玩家
        inputManager.enablePlayer = true;

        //当场景还有敌人存在就一直进行下去
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }
    }

    private IEnumerator AfterRound()
    {
        //使玩家待机
        inputManager.enablePlayer = false;

        //检查是否是第三波

        currRound++;
        //倒计时
        int leftSeconds = countdown;
        while(leftSeconds >= 0)
        {
            countdownText.text = leftSeconds + " s!";
            yield return new WaitForSeconds(1f);
            leftSeconds--;
        }

    }

    private IEnumerator RoundLoop()
    {
        yield return StartCoroutine(PreRound());
        yield return StartCoroutine(InRound());
        yield return StartCoroutine(AfterRound());

        if(tempFlag || currRound == 3)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        {
            StartCoroutine(RoundLoop());
        }
    }
}
