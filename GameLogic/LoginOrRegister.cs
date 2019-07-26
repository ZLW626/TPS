using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Network;
using Assets.Script.Common;
using System.Security.Cryptography;
using System.Text;

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
        //获取输入框的用户名和密码并加密
        string username = MD5Encryption(loginNameIF.text);
        string password = MD5Encryption(loginPasswordIF.text);
        Debug.Log(username);
        Debug.Log(password);

        MsgCSLogin msg = new MsgCSLogin(username, password);

        byte[] msgPacked = msg.Marshal();
        SocketClient.netStream.Write(msgPacked, 0, msgPacked.Length);
        
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
        if(registerPasswordIF.text.Equals(registerPasswordIF2.text))
        {
            string username = MD5Encryption(registerNameIF.text);
            string password = MD5Encryption(registerPasswordIF.text);
            //string password2 = MD5Encryption(registerPasswordIF2.text);
            Debug.Log(username);
            Debug.Log(password);
            //Debug.Log(password2);
            MsgCSRegister msg = new MsgCSRegister(username, password);
            byte[] msgPacked = msg.Marshal();
            SocketClient.netStream.Write(msgPacked, 0, msgPacked.Length);

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
