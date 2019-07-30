using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //连射机枪
    //private float timeBetweenBulltets = 0.1f;
    //private float timer = 0f;

    private Ray shootRay;
    private RaycastHit hitPoint;
    private int shootableMask;

    [SerializeField] private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        //if(Input.GetButtonDown("Fire1"))
        //{
        shootRay.origin = camera.transform.position;
        Vector3 shootDirScreen = new Vector3(
            Screen.width / 2f,
            Screen.height / 2f,
           camera.farClipPlane);
        Vector3 shootDir = camera.ScreenToWorldPoint(shootDirScreen);
        shootRay.direction = camera.transform.forward;

        if (Physics.Raycast(shootRay, out hitPoint, 200, shootableMask))
        {
            EnemyHealth enemyHealth =
                hitPoint.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                //发送玩家击中敌人的信息到服务器
                int damage = 0;
                //根据枪类型选择不同的伤害
                enemyHealth.TakeDamage(damage);
            }
        }

        //}
    }

    void ChangeGun()
    {

    }
}
