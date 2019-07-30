using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private int maxHpVal = 100;
    private int currHpVal;
    public Slider slider;
    public Image hpBarBkg;
    public Text hpValText;

    //private PlayerController playerController;
    private InputManager inputManager;
    private GameObject playerCamera;
    private GameObject tankCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.value = 1f;
        hpBarBkg.fillAmount = slider.value;
        hpValText.text = "HP: " + maxHpVal;

        //playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        playerCamera = GameObject.Find("Main Camera");
        tankCamera = GameObject.Find("TankCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerController.isInTank)//坦克模式
        //    transform.rotation = tankCamera.transform.rotation;
        //else
        //    transform.rotation = playerCamera.transform.rotation;
        if (inputManager.isOnTank)//坦克模式
            transform.rotation = tankCamera.transform.rotation;
        else
            transform.rotation = playerCamera.transform.rotation;
    }

    public void UpdateValue(int delta)
    {
        currHpVal -= delta;
        if (currHpVal > maxHpVal)
            currHpVal = maxHpVal;
        else if (currHpVal < 0)
            currHpVal = 0;

        slider.value = currHpVal / (float)maxHpVal;
        hpBarBkg.fillAmount = slider.value;
        hpValText.text = "HP: " + currHpVal.ToString();
    }
}
