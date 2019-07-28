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
        PlayerAnimate();
    }

    void PlayerAnimate()
    {
        playerAnimator.SetFloat("speedV", playerController.v);
        playerAnimator.SetFloat("speedH", playerController.h);
        //Debug.Log("v" + playerController.v);
        //Debug.Log("h" + playerController.h);

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
}
