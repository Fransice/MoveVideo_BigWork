using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class ClientServer : MonoBehaviour
{
    Socket_Cilent socket = new Socket_Cilent();
    public InputField User_name;
    public InputField User_password;
    public InputField IP;
    public InputField Port;
    private bool IsClient;
    public GameObject LoadUI;
    public GameObject Caveat;
    public void Client()
    {
        JsonData data = new JsonData();
        data["User_name"] = User_name.text;
        data["User_password"] = User_password.text;
        print(data.ToJson());
        if (!IsClient)
        {
            IsClient = true;
            socket.InitClient(IP.text, int.Parse(Port.text));
        }
        socket.ClientSendMessage(data.ToJson());
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (socket.Message.Count > 0)
        {
            string laod = socket.Message.Dequeue().ToString();
            print(laod);
            if (int.Parse(laod) == 2)
            {
                Caveat.SetActive(true);
            }
            else if (int.Parse(laod) == 1)
            {
                LoadUI.SetActive(false);
                socket.Close();
            }
        }
    }
    private void OnApplicationQuit()
    {
        print("关闭");
        socket.Close();

    }
}