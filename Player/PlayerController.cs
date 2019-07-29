using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //调节相机旋转的参数
    [SerializeField] private float mouseSensitivity = .5f;
    [SerializeField] private float smoothTime = 18f;
    [SerializeField] private float camVerticalRotThresh = 0.8f; //旋转范围
    [SerializeField] private float camVerticalRotMargin = 0.05f;//相机反向旋转的弧度大小, 此值应尽量小,防止相机抖动
    [SerializeField] private float speedAdjust = 1.5f;

    //玩家和相机的需要旋转的角度
    private Quaternion targetRotationPlayer;
    private Quaternion targetRotationCamera;
    private Vector3 camRotCenter;
    private Transform camTrans;

    //玩家上坦克
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject tankCameraObj;
    [SerializeField] private PlayerTankController playerTankController;
    [SerializeField] private Transform realTankTrans;
    public bool isInTank;
    private float canGetOnRadius = 5f;

    //通过刚体控制玩家的移动
    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private bool isJump;
    private bool isInAir;
    private float jumpForce = 60f;
    //private Animator playerAnimator;
    public float h;//键盘输入
    public float v;

    // Start is called before the first frame update
    void Start()
    {
        targetRotationPlayer = transform.localRotation;
        //camTrans = Camera.main.transform;
        camTrans = GameObject.Find("Main Camera").transform;
        targetRotationCamera = camTrans.localRotation;

        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        //playerAnimator = GetComponent<Animator>();

        isInTank = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTank)
            return;
        //Debug.Log("out tank update");
        CameraRotate();

        PlayerGetOnTank();

        if(Input.GetButtonDown("Jump") && !isJump)
        {
            Debug.Log("Jump");
            isJump = true;
        }

        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (isInTank)
            return;

        PlayerMove();
    }

    void CameraRotate()
    {
        //记录玩家的朝向
        float yOldAng = transform.eulerAngles.y;

        //获取鼠标移动
        float hMouse = Input.GetAxis("Mouse X") * mouseSensitivity;
        float vMouse = Input.GetAxis("Mouse Y") * mouseSensitivity;
        //计算玩家和相机需要旋转的角度
        targetRotationPlayer *= Quaternion.Euler(0f, hMouse, 0f);
        targetRotationCamera *= Quaternion.Euler(vMouse, 0f, 0f);
        //让玩家绕y轴旋转
        transform.localRotation = Quaternion.Slerp(transform.localRotation,
            targetRotationPlayer, Time.deltaTime * smoothTime);
        //设置相机的旋转中心
        camRotCenter.x = transform.position.x;
        camRotCenter.y = transform.position.y + 1.5f;//相机的旋转中心稍稍高于地面
        camRotCenter.z = transform.position.z;
        //控制相机的旋转范围
        if (camTrans.forward.y > camVerticalRotThresh)//若超出范围则反向旋转
            camTrans.RotateAround(camRotCenter,
                transform.right,//Vector3.Cross(transform.up, transform.forward),
                camVerticalRotMargin);
        else if(camTrans.forward.y < -camVerticalRotThresh)
            camTrans.RotateAround(camRotCenter,
                Vector3.Cross(transform.up, transform.forward),
                -camVerticalRotMargin);
        else
            camTrans.RotateAround(camRotCenter,
                Vector3.Cross(transform.up, transform.forward),
                vMouse);
            

        //旋转玩家的刚体速度方向
        Quaternion yRotationRigidbody =
            Quaternion.AngleAxis(transform.eulerAngles.y - yOldAng,
            Vector3.up);
        playerRigidbody.velocity = yRotationRigidbody * playerRigidbody.velocity;
        
    }

    void PlayerMove()
    {
        //获取键盘输入
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        bool run = Input.GetKey(KeyCode.LeftShift);

        //计算玩家移动速度
        float speed = Mathf.Sqrt(h * h + v * v);
        if (speed > 1f)
            speed = 1f;

        if (run)
            speed = speed * speedAdjust;
        else
            speed = speed * speedAdjust * 0.5f;

        //计算玩家移动向量
        Vector3 movement = (transform.forward * v +
            transform.right * h).normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);


        //判断玩家是否在空中
        isInAir = IsInAir();
        //Debug.Log("isInAir" + isInAir);
        if (!isInAir)
        {
            playerRigidbody.drag = 5f;
            if(isJump)
            {
                Debug.Log("Jump");
                playerRigidbody.drag = 0f;
                playerRigidbody.velocity = new Vector3(
                    playerRigidbody.velocity.x,
                    0f,
                    playerRigidbody.velocity.z);
                playerRigidbody.AddForce(
                    new Vector3(0f, jumpForce, 0f), 
                    ForceMode.Impulse);
            }
        }
        else
        {
            playerRigidbody.drag = 0f;
        }
        isJump = false;
    }

    void PlayerGetOnTank()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(Vector2.Distance(
                new Vector2(transform.position.x, transform.position.y),
                new Vector2(realTankTrans.position.x, realTankTrans.position.y))
                < canGetOnRadius)
            {
                Debug.Log("get on");
                mainCameraObj.SetActive(false);
                tankCameraObj.SetActive(true);
                isInTank = true;
                playerTankController.isOutTank = false;
                playerTankController.hasPlayer = true;
                playerTankController.DisplayAimImage();
                gameObject.SetActive(false);
            }
            
        }
    }

    bool IsInAir()
    {
        //通过判断玩家的碰撞体是否与周围物体有接触来判断玩家是否在空中
        RaycastHit hitInfo;
        //碰撞检测的中心位于碰撞体的中心,因此设置中心的位置为transform之上
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        if (Physics.SphereCast(playerCollider.center, playerCollider.radius,
            Vector3.down, out hitInfo, 
            playerCollider.height / 2f - 0f,
            Physics.AllLayers, QueryTriggerInteraction.Ignore))
            return false;
        else
            return true;
        

    }
}
