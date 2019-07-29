using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Network;
using Assets.Script.Common;

public class MessageManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ProcessMessage()
    {
        byte[] dataReceivedNoHead = SocketClient.RemoveDataHead();
    }
}
