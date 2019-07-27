using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //调节相机旋转的参数
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float smoothTime = 18f;
    [SerializeField] private float camVerticalRotThresh = 0.8f; //旋转范围
    [SerializeField] private float camVerticalRotMargin = 0.05f;//相机反向旋转的弧度大小, 此值应尽量小,防止相机抖动
    [SerializeField] private float speedAdjust = 1.5f;

    //玩家和相机的需要旋转的角度
    private Quaternion targetRotationPlayer;
    private Quaternion targetRotationCamera;
    private Vector3 camRotCenter;
    private Transform camTrans;


    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private bool isJump;
    private bool isInAir;
    private float jumpForce = 60f;

    //[SerializeField] private CharacterController playerController;
    //private Vector2 inputXYValue;
    //private Vector3 movement;
    //private float speed = 4f;
    //private float speedJump = 7f;
    //private float gravity = 15f;
    //private float jumpForce = 80f;
    //private bool isJumping = false;
    //private bool isMoving = false;
    //private CollisionFlags flags;
    // Start is called before the first frame update
    void Start()
    {
        targetRotationPlayer = transform.localRotation;
        camTrans = Camera.main.transform;
        targetRotationCamera = camTrans.localRotation;

        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotate();

        if(Input.GetButtonDown("Jump") && !isJump)
        {
            Debug.Log("Jump");
            isJump = true;
        }
        //PlayerMove();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
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
                Vector3.Cross(transform.up, transform.forward),
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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //计算玩家移动速度
        float speed = Mathf.Sqrt(h * h + v * v) * speedAdjust;

        //计算玩家移动向量
        Vector3 movement = (transform.forward * v +
            transform.right * h).normalized * speed * Time.deltaTime;
        //movement = movement.normalized * speed;
        playerRigidbody.MovePosition(transform.position + movement);
        //transform.Translate()

        //施加一个力，使玩家运动
        //if（playerRigidbody.velocity.sqrMagnitude < 
        //(sp)
        //判断玩家是否在空中
        isInAir = IsInAir();
        Debug.Log("isInAir" + isInAir);
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
    //void PlayerMove()
    //{


    //    inputXYValue.x = Input.GetAxis("Horizontal");
    //    inputXYValue.y = Input.GetAxis("Vertical");

    //    if(Mathf.Abs(inputXYValue.x) > 0.1f || Mathf.Abs(inputXYValue.y) > 0.1f)
    //    {
    //        isMoving = true;
    //    }

    //    //movement.x = inputXYValue.x;
    //    //movement.y = 0f;
    //    //movement.z = inputXYValue.y;
    //    //movement *= speed;


    //    if (Input.GetButton("Jump") && !isJumping)
    //    {
    //        isJumping = true;
    //        movement.y = speedJump;
    //    }
    //    //if(isJumping)
    //    //{
    //    //    movement.y += jumpForce * Time.deltaTime;
    //    //    if()
    //    //}


    //    if (isMoving)
    //    {
    //        movement.x = inputXYValue.x;
    //        //movement.y = 0f;
    //        movement.z = inputXYValue.y;
    //        movement *= speed;
    //        //playerController.Move(movement * Time.deltaTime);
    //        isMoving = false;
    //    }

    //    if (isJumping)
    //    {
    //        movement.y -= gravity * Time.deltaTime;
    //        flags = playerController.Move(movement * Time.deltaTime);
    //        if (flags == CollisionFlags.Below)
    //        {
    //            isJumping = false;
    //        }
    //    }
    //    else
    //    {
    //        playerController.Move(movement * Time.deltaTime);
    //    }
    //}

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
