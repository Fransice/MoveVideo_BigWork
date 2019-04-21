
//
//   █████▒█    ██  ▄████▄   ██ ▄█▀       ██████╗ ██╗   ██╗ ██████╗
// ▓██   ▒ ██  ▓██▒▒██▀ ▀█   ██▄█▒        ██╔══██╗██║   ██║██╔════╝
// ▒████ ░▓██  ▒██░▒▓█    ▄ ▓███▄░        ██████╔╝██║   ██║██║  ███╗
// ░▓█▒  ░▓▓█  ░██░▒▓▓▄ ▄██▒▓██ █▄        ██╔══██╗██║   ██║██║   ██║
// ░▒█░   ▒▒█████▓ ▒ ▓███▀ ░▒██▒ █▄       ██████╔╝╚██████╔╝╚██████╔╝
//  ▒ ░   ░▒▓▒ ▒ ▒ ░ ░▒ ▒  ░▒ ▒▒ ▓▒       ╚═════╝  ╚═════╝  ╚═════╝
//  ░     ░░▒░ ░ ░   ░  ▒   ░ ░▒ ▒░
//  ░ ░    ░░░ ░ ░ ░        ░ ░░ ░
//           ░     ░ ░      ░  ░
// 
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using System.Collections.Generic;
public class Socket_Cilent
{
    #region
    public Queue Message = new Queue();//消息队列
    public Queue Port = new Queue();//消息队列
    [HideInInspector]
    Thread Data_Managerl;
    public Socket clientSocket;
    public bool IS_OnLine;
    private int startIndex = 0;
    public int StartIndex;
    IPEndPoint clientEP;
    float T;
    float time;
    private List<byte> probuffer;
    byte[] clientBuffer = new byte[2048];
    byte[] serverBuffer = new byte[2048];
    bool IS_Once = true;
    string Client_IP;
    int Client_Port;
    public void InitClient(string ip, int port)
    {
        if (IS_Once)
        {
            Client_IP = ip;
            Client_Port = port;
            IS_Once = false;
        }
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientEP = new IPEndPoint(IPAddress.Parse(ip), port);
            clientSocket.Connect(clientEP);
            Debug.Log("端口号是:" + clientEP.Port + "IP是:" + clientEP.Address);
            clientSocket.BeginReceive(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None, new System.AsyncCallback(clientReceive), this.clientSocket);
        }
        catch (System.Exception es)
        {
            Debug.Log("无法连接服务器..." + es);
            Close();
        }
    }
    void clientReceive(System.IAsyncResult ar)
    {
        Debug.Log("接受到消息");
        Socket workingSocket = ar.AsyncState as Socket;
        int byteCount = 0;
        try
        {
            byteCount = workingSocket.EndReceive(ar);
        }
        catch (SocketException se)
        {
            Debug.Log("已断开连接....正在重连...");
            Close();
            //做重连操作
        }
        try
        {
            string message = Encoding.UTF8.GetString(clientBuffer);
            Debug.Log("有消息传入  " + message);
            Message.Enqueue(message);//消息传入队列中
        }
        catch (System.Exception ex)
        {


        }
        //继续接收
        clientSocket.BeginReceive(clientBuffer, 0, this.clientBuffer.Length, SocketFlags.None, new System.AsyncCallback(clientReceive), this.clientSocket);
    }

    //发送数据
    public void ClientSendMessage(string msg)
    {
        Debug.Log("ssssss");
        //将要发送的字符串消息转换成BYTE数组
        serverBuffer = UTF8Encoding.UTF8.GetBytes(msg);
        clientSocket.BeginSend(serverBuffer, 0, this.serverBuffer.Length, SocketFlags.None, new System.AsyncCallback(SendMsg), this.clientSocket);
        // Debug.Log("端口号为: " + clientEP.Port + "  的连接正常");
        Port.Enqueue(clientEP.Port);
    }

    void SendMsg(System.IAsyncResult ar)
    {
        Debug.Log("dddddd");
        Socket workingSocket = ar.AsyncState as Socket;
        workingSocket.EndSend(ar);
    }
    public void Close()
    {
        clientSocket.Close();
    }
    #endregion
}
