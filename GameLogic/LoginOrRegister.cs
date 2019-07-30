using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Network;
using Assets.Script.Common;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;

public class LoginOrRegister : MonoBehaviour
{
    //登录游戏UI控件
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private InputField loginNameIF;
    [SerializeField] private InputField loginPasswordIF;

    //创建角色UI控件
    [SerializeField] private Button registerButton;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private InputField registerNameIF;
    [SerializeField] private InputField registerPasswordIF;
    [SerializeField] private InputField registerPasswordIF2;

    //byte[] dataReceived;

    // Start is called before the first frame update
    void Start()
    {
        //获取UI控件实例
        //loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        //loginPanel = GameObject.Find("LoginPanel");
        //loginNameIF = GameObject.Find("LoginNameInputField").GetComponent<InputField>();
        //loginPasswordIF = GameObject.Find("LoginPwdInputField").GetComponent<InputField>();
        loginPanel.SetActive(false);

        //registerButton = GameObject.Find("RegisterButton").GetComponent<Button>();
        //registerPanel = GameObject.Find("RegisterPanel");
        //registerNameIF = GameObject.Find("RegisterNameInputField").GetComponent<InputField>();
        //registerPasswordIF = GameObject.Find("RegisterPwdInputField").GetComponent<InputField>();
        //registerPasswordIF2 = GameObject.Find("RegisterPwdInputField2").GetComponent<InputField>();
        registerPanel.SetActive(false);
    }

    public void OnLoginBtnClicked()
    {
        loginPanel.SetActive(true);
    }

    public void OnLoginOKBtnClicked()
    {
        //获取输入框的用户名和密码, 并加密密码
        //string username = MD5Encryption(loginNameIF.text);
        string username = loginNameIF.text;
        string password = MD5Encryption(loginPasswordIF.text);
        //Debug.Log(username);
        //Debug.Log(password);

        MsgCSLogin msg = new MsgCSLogin(username, password);

        byte[] dataToSend = msg.Marshal();
        SocketClient.netStream.Write(dataToSend, 0, dataToSend.Length);

        //byte[] dataLenReceived = new byte[4];
        //SocketClient.netStream.Read(dataLenReceived, 0, 4);

        //byte[] dataReceived = { };
        byte[] dataReceivedNoHead = SocketClient.RemoveDataHead();
        MsgSCBase msgSCBase = new UnifromUnmarshal().Unmarshal(dataReceivedNoHead);

        MsgSCLoginConfirm msgConfirm = (MsgSCLoginConfirm)msgSCBase;
        //MsgSCBase msgComfirm = new UnifromUnmarshal().Unmarshal(dataReceivedNoHead);
        int comfirmCode = msgConfirm.confirm;
        if (comfirmCode == 0)
        {
            Debug.Log("login successfully!");
            loginPanel.SetActive(false);
            PlayerPrefs.SetInt("hp", msgConfirm.hp);
            PlayerPrefs.SetInt("money", msgConfirm.money);
            PlayerPrefs.SetInt("ammo", msgConfirm.ammo);
            PlayerPrefs.SetInt("grenade", msgConfirm.grenade);
            PlayerPrefs.SetFloat("shell", msgConfirm.shell);

            SceneManager.LoadScene("BattlefieldScene");
        }
        else
        {
            loginNameIF.text = "";
            loginPasswordIF.text = "";
            Debug.Log("login failed!");
            loginPanel.SetActive(false);
        }
            


    }
    public void OnLoginCancelBtnClicked()
    {
        loginNameIF.text = "";
        loginPasswordIF.text = "";
        loginPanel.SetActive(false);
    }

    public void OnRegisterBtnClicked()
    {
        registerPanel.SetActive(true);
    }
    public void OnRegisterOKBtnClicked()
    {
        //比较两次输入的密码是否一致
        if(registerPasswordIF.text.Equals(registerPasswordIF2.text))
        {
            //如果一致, 获取输入框的用户名和密码,并加密密码
            //string username = MD5Encryption(registerNameIF.text);
            string username = registerNameIF.text;
            string password = MD5Encryption(registerPasswordIF.text);
            //string password2 = MD5Encryption(registerPasswordIF2.text);
            //Debug.Log(username);
            //Debug.Log(password);
            //Debug.Log(password2);
            MsgCSRegister msg = new MsgCSRegister(username, password);
            byte[] msgPacked = msg.Marshal();
            SocketClient.netStream.Write(msgPacked, 0, msgPacked.Length);

            byte[] dataReceivedNoHead = SocketClient.RemoveDataHead();
            MsgSCBase msgComfirm = new UnifromUnmarshal().Unmarshal(dataReceivedNoHead);
            int comfirmCode = ((MsgSCConfirm)msgComfirm).confirm;
            if (comfirmCode == 1)
            {
                Debug.Log("register successfully!");
                registerPanel.SetActive(false);
            }
            else
                Debug.Log("register failed!");

        }
        else
        {
            Debug.Log("passwords mismatch!");
        }
    }
    public void OnRegisterCancelBtnClicked()
    {
        registerNameIF.text = "";
        registerPasswordIF.text = "";
        registerPasswordIF2.text = "";
        registerPanel.SetActive(false);
    }

    private string MD5Encryption(string plainText)
    {
        MD5 md5 = MD5.Create();
        byte[] bytesPlainText = Encoding.Default.GetBytes(plainText);
        byte[] bytesPlainTextEncrypted = md5.ComputeHash(bytesPlainText);

        StringBuilder stringBuilder = new StringBuilder();
        for(int i = 0;i < bytesPlainTextEncrypted.Length;++i)
        {
            stringBuilder.Append(bytesPlainTextEncrypted[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}
