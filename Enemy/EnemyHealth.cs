using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    private Slider slider;
    private Image hpImage;
    private Text hpText;
    private PlayerMoney playerMoney;
    private int enemyVal = 10;

    // Start is called before the first frame update
    void Start()
    {
        playerMoney = GameObject.Find("Player").GetComponent<PlayerMoney>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            //敌人死亡
            //......

            //增加玩家金钱
            playerMoney.AddMoney(enemyVal);
        }
            

    }
}
