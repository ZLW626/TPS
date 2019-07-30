using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Animator playerAnimator;
    private int[] weaponID = { 0, 1 };
    private int currWeaponID = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.gameObject.GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //PlayerAnimate();
    }

    void PlayerAnimate()
    {
        //往后跑时动画需要调整
        float adjustRunWalk = 1f;
        bool run = Input.GetKey(KeyCode.LeftShift);
        //if (!run)
        //    adjustRunWalk *= .5f;
        //playerAnimator.SetFloat("speedV", playerController.v * adjustRunWalk);
        //if(playerController.v < 0)
        //    playerAnimator.SetFloat("speedH", -playerController.h * adjustRunWalk);
        //else
        //    playerAnimator.SetFloat("speedH", playerController.h * adjustRunWalk);
        ////Debug.Log("v" + playerController.v);
        ////Debug.Log("h" + playerController.h);

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerAnimator.SetTrigger("switchWeapon");
            currWeaponID = (currWeaponID + 1) % 2;
            playerAnimator.SetInteger("gunFlag", currWeaponID);
        }
        if (Input.GetKeyDown(KeyCode.R))
            playerAnimator.SetTrigger("reload");
        if (Input.GetKeyDown(KeyCode.G))
            playerAnimator.SetBool("grenadeMode", !playerAnimator.GetBool("grenadeMode"));
        if (Input.GetButton("Fire1"))
            playerAnimator.SetTrigger("gunShoot");
    }

    public void Shoot()
    {
        playerAnimator.SetTrigger("gunShoot");
    }

    public void Move(float h, float v, bool run)
    {
        float adjustRunWalk = 1f;
        if (!run)
            adjustRunWalk *= .5f;
        playerAnimator.SetFloat("speedV", v * adjustRunWalk);
        if (playerController.v < 0)
            playerAnimator.SetFloat("speedH", -h * adjustRunWalk);
        else
            playerAnimator.SetFloat("speedH", h * adjustRunWalk);
        //Debug.Log("v" + playerController.v);
        //Debug.Log("h" + playerController.h);
    }


}
