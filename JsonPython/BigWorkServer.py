import socket
import json


def Main():
    Loads = "1"
    NoLoads = "2"
    # 创建服务端的socket对象socketserver
    socketserver = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    host = '127.0.0.1'
    port = 9092
    # 绑定地址（包括ip地址会端口号）
    socketserver.bind((host, port))
    # 设置监听
    socketserver.listen(5)
    # 等待客户端的连接
    # 注意：accept()函数会返回一个元组
    # 元素1为客户端的socket对象，元素2为客户端的地址(ip地址，端口号)
    clientsocket, addr = socketserver.accept()

    # while循环是为了能让对话一直进行，直到客户端输入q
    while True:
        # 接收客户端的请求
        recvmsg = clientsocket.recv(1024)
        # 把接收到的数据进行解码
        strData = recvmsg.decode("ISO-8859-1")
        # 判断客户端是否发送q，是就退出此次对话
        if strData == 'q':
            break
        print("收到:" + strData)
        jsonSend = json.loads(strData)
        json_Name = jsonSend["User_name"]
        json_Password = jsonSend["User_password"]

        with open('UserData.json', encoding='utf-8') as f:
            line = f.read()
            jsonn = json.loads(line)
            f.close()
            for i in range(len(jsonn["User"])):
                User_Name = jsonn["User"][i]["User_name"]
                User_Passwoed = jsonn["User"][i]["User_password"]

                if User_Name == json_Name and User_Passwoed == json_Password:  # 输入正确
                    print("密码正确")
                    clientsocket.send(Loads.encode("utf-8"))
                    clientsocket.close()
                    break
                else:
                    print("输入不正确")
                    clientsocket.send(NoLoads.encode("utf-8"))

        # 对要发送的数据进行编码
        # clientsocket.send(strData.encode("utf-8"))
    socketserver.close()


if __name__ == '__main__':
    Main()
