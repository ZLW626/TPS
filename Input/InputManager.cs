using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerTankController playerTankController;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerTankFiring playerTankFiring;

    public bool isOnTank = false;
    private bool isOtherPlayerOnTank = false;
    private bool isJump;
    private float waitingTime = 0f; //坦克下次射击等待时间

    public bool enablePlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!enablePlayer)
            return;
        //获取鼠标移动
        float hMouse = Input.GetAxis("Mouse X");
        float vMouse = Input.GetAxis("Mouse Y");

        //玩家和坦克的第三人称视角
        if(isOnTank)
        {
            playerTankController.CameraRotate(hMouse);
            playerTankController.TankTurretRotate();
        }
        else
        {
            playerController.CameraRotate(hMouse, vMouse);
        }

        waitingTime += Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            if(isOnTank)
            {
                Debug.Log("Fire1");
                playerTankFiring.TankFire(ref waitingTime);
            }
            else
            {
                playerShoot.Shoot();
                playerAnimation.Shoot();
            }
        }

        //if(Input.GetKeyDown(KeyCode.T) && !isOtherPlayerOnTank && !isOnTank)
        //{
        //    playerController.PlayerGetOnTank();
        //    isOnTank = true;
        //}
        //else if(Input.GetKeyDown(KeyCode.F))
        //{
        //    playerTankController.PlayerGetOffTank();
        //    isOnTank = false;
        //}

        if(Input.GetKeyDown(KeyCode.T))
        {
            if(isOnTank)
            {
                playerTankController.PlayerGetOffTank();
                isOnTank = false;
            }
            else
            {
                if(!isOtherPlayerOnTank)
                {
                    playerController.PlayerGetOnTank();
                    isOnTank = true;
                    //isOtherPlayerOnTank = true;
                }
                
            }
        }

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            //Debug.Log("Jump");
            isJump = true;
        }

        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (!enablePlayer)
            return;

        //获取键盘输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if(isOnTank)
        {
            playerTankController.TankMove(h, v);
        }
        else
        {
            playerController.PlayerMove(h, v, isRunning, ref isJump);
            playerAnimation.Move(h, v, isRunning);
        }
    }
}
