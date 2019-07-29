using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankFiring : MonoBehaviour
{
    [SerializeField] private Rigidbody shellRigibodyPrefab;
    [SerializeField] private Transform fireTransform;
    private PlayerTankController playerTankController;
    private float fireForce = 12f;

    // Start is called before the first frame update
    void Start()
    {
        playerTankController = GetComponent<PlayerTankController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerTankController.hasPlayer) 
            TankFire();
    }

    void TankFire()
    {
        Rigidbody shellRigidbody = Instantiate(shellRigibodyPrefab,
            fireTransform.position, fireTransform.rotation) as Rigidbody;

        shellRigidbody.velocity = fireTransform.forward * fireForce;
    }
}
