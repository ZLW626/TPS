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
    [SerializeField] private Transform realTankTrans;
    private float speedRotate = 30f;

    //炮台转动
    [SerializeField] private Transform tankTurretTrans;
    [SerializeField] private GameObject crossImage;
    [SerializeField] private GameObject circleImage;
    private bool isTurretRotating = false;
    private float currAngle;
    private float oldAngle;
    private Vector3 oldForward;

    private Quaternion targetRotation;
    [SerializeField] private GameObject fakeObject;
    private bool shouldRecord = true;
    [SerializeField] private Transform fireTransform;

    //玩家下坦克
    [SerializeField] private GameObject playerObj;
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject tankCameraObj;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform getOffPoint;
    public bool isOutTank;
    public bool hasPlayer;

    // Start is called before the first frame update
    void Start()
    {
        isOutTank = true;
        hasPlayer = false;
        targetRotationCamera = transform.localRotation;

        UndisplayAimImage();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOutTank)
            return;
        //CameraRotate();

        //TankTurretRotate();

        //PlayerGetOffTank();
    }

    private void FixedUpdate()
    {
        //if (isOutTank)
        //    return;
        //TankMove();
    }

    public void CameraRotate(float hMouse)
    {
        ////获取鼠标移动
        //hMouse = Input.GetAxis("Mouse X") * mouseSensitivity;

        //设置相机的旋转中心
        tankCamRotCenter.x = realTankTrans.position.x;
        tankCamRotCenter.y = realTankTrans.position.y;
        tankCamRotCenter.z = realTankTrans.position.z;

        //旋转相机
        tankCameraTrans.RotateAround(tankCamRotCenter, Vector3.up, hMouse);
        fakeObject.transform.RotateAround(tankCamRotCenter, Vector3.up, hMouse);
    }

    public void TankTurretRotate()
    {
        StartCoroutine(TurretRotateCore(oldAngle));

        if (Vector3.Angle(tankTurretTrans.forward, fakeObject.transform.forward) < 2f)
        {
            crossImage.SetActive(true);
        }
        else
        {
            crossImage.SetActive(false);
        }
    }

    private IEnumerator TurretRotateCore(float angle)
    {
        yield return new WaitForSeconds(1);

        tankTurretTrans.rotation = Quaternion.Slerp(tankTurretTrans.rotation, 
            fakeObject.transform.rotation, Time.deltaTime);

        if (Quaternion.Angle(tankTurretTrans.rotation, fakeObject.transform.rotation) < 2f)
            tankTurretTrans.rotation = tankTurretTrans.rotation;
    }

    public void TankMove(float h, float v)
    {
        ////获取键盘输入
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");
        if (v < 0)
            h = -h;

        //旋转坦克
        Quaternion tankRotation = Quaternion.Euler(0f,
            h * speedRotate * Time.deltaTime, 0f);
        realTankTrans.Rotate(realTankTrans.up, h * speedRotate * Time.deltaTime);

        //计算坦克移动向量并移动坦克
        Vector3 movement = realTankTrans.transform.forward * v * speed * Time.deltaTime;
        tankRigidbody.MovePosition(tankRigidbody.position + movement);
    }

    public void PlayerGetOffTank()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        //{
            Debug.Log("get off");
            playerObj.SetActive(true);
            mainCameraObj.SetActive(true);

            playerObj.transform.position = getOffPoint.position;
            tankCameraObj.SetActive(false);
            //isOutTank = true;
            //hasPlayer = false;
            //playerController.isInTank = false;
            UndisplayAimImage();

        //}
    }

    public void DisplayAimImage()
    {
        crossImage.SetActive(true);
        circleImage.SetActive(true);
    }

    public void UndisplayAimImage()
    {
        crossImage.SetActive(false);
        circleImage.SetActive(false);
    }




}
