using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;

public class TestSocket : MonoBehaviour
{
    [SerializeField]
    private string ip;

    [SerializeField]
    private int port;

    [SerializeField]
    private int[] allChannels;

    private TCPClient tcpClient;

    void Awake()
    {
        tcpClient = new TCPClient();
        tcpClient.Connect(ip, port);
    }

    public void Send(IData data)
    {
        tcpClient.Send(data);
    }
   
    public void SetCallback(UnityAction<string> callback)
    {
        tcpClient.receiveCallback = callback;
    }

    /*
    void Update()
    {
        if(textQueue.Count > 0)
        {
            textList.Add(textQueue.Dequeue());
        }
    }

    public void OnChatCallback(string inputText)
    {
        ChatData chatData = JsonUtility.FromJson<ChatData>(inputText);
        textQueue.Enqueue(chatData.msg);
    }
    */
    public class TCPClient
    {
        public UnityAction<string> receiveCallback;

        private Socket socket = null;

        /// <summary>
        /// 数据缓冲
        /// </summary>
        private byte[] msgBuffer;

        /// <summary>
        /// 最大数据缓冲
        /// </summary>
        private int maxLength = 1024;

        /// <summary>
        /// 用于解决粘包
        /// </summary>
        private StringBuilder msgBuilder = new StringBuilder();

        private string terminateStr = "\r\n";

        public TCPClient()
        {
            msgBuffer = new byte[maxLength];
        }

        public bool Connect(string ip, int port)
        {
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(address, port);

            try
            {
                while (true)
                {
                    socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    socket.BeginConnect(endPoint, new System.AsyncCallback(ConnectionCallback), socket);
                    break;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            
            return true;
        }

        private void ConnectionCallback(System.IAsyncResult asy)
        {
            Debug.Log("callback");

            Socket handlerSocket = (Socket)asy.AsyncState;

            handlerSocket.EndConnect(asy);
            handlerSocket.SendTimeout = 3;
            handlerSocket.ReceiveTimeout = 3;

            socket.BeginReceive(msgBuffer, 0, msgBuffer.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), socket);
        }

        private void ReceiveCallback(System.IAsyncResult result)
        {
            try
            {
                int byteLength = socket.EndReceive(result);

                if(byteLength > 0)
                {
                    byte[] data = new byte[byteLength];
                    System.Array.Copy(msgBuffer, 0, data, 0, byteLength);
                    string msgStr = Encoding.UTF8.GetString(data);
                    Debug.Log(msgStr);

                    for (int i = 0; i < msgStr.Length;)
                    {
                        if (i <= msgStr.Length - terminateStr.Length)
                        {
                            if (msgStr.Substring(i, terminateStr.Length) != terminateStr)
                            {
                                msgBuilder.Append(msgStr[i]);
                                i++;
                            }
                            else
                            {
                                //执行回调函数
                                receiveCallback(msgBuilder.ToString());
                                //清空string builder
                                msgBuilder.Remove(0, msgBuilder.Length);
                                i += terminateStr.Length;
                            }
                        }
                        else
                        {
                            msgBuilder.Append(msgStr[i]);
                            i++;
                        }
                    }
                    socket.BeginReceive(msgBuffer, 0, msgBuffer.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), socket);
                }
                else
                {
                    Debug.Log("Connection Lost!");
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void Send(IData data)
        {
            if (!socket.Connected)
            {
                Debug.LogError("Connection lost!");
            }

            NetworkStream netStream;
            lock (socket)
            {
                netStream = new NetworkStream(socket);
            }
            if (netStream.CanWrite)
            {
                try
                {
                    string msg = JsonWrapperArray<string>.ToJson(data);

                    byte[] bytes = Encoding.UTF8.GetBytes(msg + "\r\n");
                    netStream.BeginWrite(bytes, 0, bytes.Length, SendCallback, netStream);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        private void SendCallback(System.IAsyncResult result)
        {
            NetworkStream netStream = (NetworkStream)result.AsyncState;

            try
            {
                Debug.Log("send back");
                netStream.EndWrite(result);
                netStream.Flush();
                netStream.Close();
            }
            catch(System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
