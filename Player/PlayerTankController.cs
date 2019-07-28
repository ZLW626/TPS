using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    //相机旋转
    [SerializeField] private Camera tankCamera;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float smoothTime = 180f;
    private float hMouse;
    private Quaternion targetRotationCamera;
    private Vector3 tankCamRotCenter;
    [SerializeField] private Transform tankCameraTrans;

    //坦克运动
    [SerializeField] private float speed = 15f;
    //[SerializeField] private Rigidbody realTankRigidbody;
    [SerializeField] private Rigidbody tankRigidbody;
    [SerializeField] private Transform realTank;
    private float speedRotate = 30f;

    //炮台转动
    [SerializeField] private Transform tankTurret;
    [SerializeField] private GameObject crossImage;
    private bool isTurretRotating = false;
    private float currAngle;
    private float oldAngle;
    private Vector3 oldForward;
    //private Vector3 turretForward;
    //private Vector3 cameraForward;
    private Quaternion targetRotation;
    [SerializeField] private GameObject fakeObject;
    private bool shouldRecord = true;

    //玩家下坦克
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject tankCameraObj;
    [SerializeField] private PlayerController playerController;
    public bool isOutTank;

    // Start is called before the first frame update
    void Start()
    {
        isOutTank = true;
        //tankCamera = GameObject.Find("TankCamera").GetComponent<Camera>();
        //tankTurret = GameObject.Find("TankTurret").transform;
        //crossImage = GameObject.Find("")
        targetRotationCamera = transform.localRotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (isOutTank)
            return;
        //Debug.Log("in tank update");
        CameraRotate();

        TankTurretRotate();

        PlayerGetOffTank();
    }

    private void FixedUpdate()
    {
        if (isOutTank)
            return;
        TankMove();
    }

    void CameraRotate()
    {
        //获取鼠标移动
        hMouse = Input.GetAxis("Mouse X") * mouseSensitivity;

        //设置相机的旋转中心
        tankCamRotCenter.x = realTank.position.x;
        tankCamRotCenter.y = realTank.position.y;
        tankCamRotCenter.z = realTank.position.z;

        //旋转相机
        tankCameraTrans.RotateAround(tankCamRotCenter, Vector3.up, hMouse);
        fakeObject.transform.RotateAround(tankCamRotCenter, Vector3.up, hMouse);

        ////计算坦克和相机整体需要旋转的角度
        //targetRotationCamera *= Quaternion.Euler(0f, hMouse, 0f);
        ////让坦克和相机整体绕y轴旋转
        //transform.localRotation = Quaternion.Slerp(transform.localRotation,
        //    targetRotationCamera, Time.deltaTime * smoothTime);
    }

    void TankTurretRotate()
    {
        //tankTurret.rotation = fakeObject.transform.rotation;
        //currAngle = Vector3.Angle(tankTurret.forward, fakeObject.transform.forward);

        //if(currAngle > 1f && shouldRecord)
        //{
        //    oldAngle = currAngle;
        //    oldForward = new Vector3(fakeObject.transform.forward.x,
        //        fakeObject.transform.forward.y,
        //        fakeObject.transform.forward.z);
        //    shouldRecord = false;
        //}

        StartCoroutine(TurretRotateCore(oldAngle));

        //if (Vector3.Angle(tankTurret.forward, oldForward) < 2f)
        //    shouldRecord = true;

        //Debug.Log("currAngle: " + currAngle);
        //Debug.Log("oldAngle: " + oldAngle);
        //Debug.Log("shouldRecord: " + shouldRecord);

        if (!isTurretRotating)
        {
            //currAngle = Vector3.Angle(tankTurret.forward, fakeObject.transform.forward);
            //targetRotation = Quaternion.Euler(0f, currAngle, 0f);
            //targetRotation = fakeObject.transform.rotation;
            //Debug.Log("recal");
            //Vector3 turretForward = tankTurret.forward;
            //Vector3 cameraForward = new Vector3(tankCameraTrans.forward.x,
            //    0f, tankCameraTrans.forward.z);
            //currAngle = Vector3.Angle(turretForward, cameraForward);
            //Debug.Log(tankTurret.forward);
            //Debug.Log(cameraForward);
            //Debug.Log("recal: " + currAngle);
        }

        if (Vector3.Angle(tankTurret.forward, fakeObject.transform.forward) < 2f)
        {
            crossImage.SetActive(true);
        }
        else
        {
            crossImage.SetActive(false);
        }



        if (currAngle > 2f)
        //if (Quaternion.Angle(targetRotation, tankTurret.rotation) > 1)
        {
            //Debug.Log("rotate: " + currAngle);
            //tankTurret.rotation = targetRotation;

            //StartCoroutine(TurretRotateCore(currAngle));
            //isTurretRotating = true;
        }
    }

    private IEnumerator TurretRotateCore(float angle)
    {
        yield return new WaitForSeconds(1);
        //targetRotation = Quaternion.Euler(0f, angle, 0f);
        tankTurret.rotation = Quaternion.Slerp(tankTurret.rotation, fakeObject.transform.rotation, Time.deltaTime * 1f);
        //tankTurret.rotation = fakeObject.transform.rotation;
        //if (Vector3.Angle(turretForward, cameraForward) < 2f)
        //if(Quaternion.Angle(targetRotation, tankTurret.rotation) < 1)
        //Debug.Log("rotate: " + angle);
        if (Vector3.Angle(tankTurret.forward, fakeObject.transform.forward) < 2f)
        {
            //Debug.Log("reach: " + Quaternion.Angle(targetRotation, tankTurret.rotation));
            //tankTurret.rotation = targetRotation;
            //isTurretRotating = false;
        }

        //tankTurret.Rotate(tankTurret.up, angle);
    }

    void TankMove()
    {
        //获取键盘输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //旋转坦克
        Quaternion tankRotation = Quaternion.Euler(0f,
            h * speedRotate * Time.deltaTime, 0f);
        realTank.Rotate(realTank.up, h * speedRotate * Time.deltaTime);
        //realTankRigidbody.MoveRotation(realTankRigidbody.rotation * tankRotation);

        //计算坦克移动速度
        //float speed = v * speedAdjust;

        //计算坦克移动向量并移动坦克
        Vector3 movement = realTank.transform.forward * v * speed * Time.deltaTime;
        //(transform.forward * v +
        //    transform.right * h).normalized * speed * Time.deltaTime;
        tankRigidbody.MovePosition(tankRigidbody.position + movement);
        //realTankRigidbody.MovePosition(realTankRigidbody.position + movement);
        ////旋转坦克
        //if( Mathf.Abs(h) > 0.1f)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(movement);
        //    realTank.rotation = Quaternion.RotateTowards(
        //        realTank.rotation,
        //        newRotation,
        //        speedRotate * Time.deltaTime);
        //}


    }

    void PlayerGetOffTank()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("get off");
            mainCameraObj.SetActive(true);
            tankCameraObj.SetActive(false);
            isOutTank = true;
            playerController.isInTank = false;
        }
    }




}
