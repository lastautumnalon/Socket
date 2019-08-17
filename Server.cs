using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;

namespace SocketServer
{
    class Program
    {
        static IPAddress IP;
        static int Port;
        static Socket Server=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        static Dictionary<string, Socket> connections = new Dictionary<string, Socket>();
        static void Main(string[] args)
        {
            Console.WriteLine("IP:");
            IP = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Port:");
            Port = int.Parse(Console.ReadLine());
            Server.Bind(new IPEndPoint(IP, Port));
            Thread ReceiveSendTh = new Thread(ReceiveSend);
            ReceiveSendTh.Start();
        }
        private static void ReceiveSend()
        {
            Server.Listen(10);
            Socket connection = null;
     
            while (true)
            {
                try
                {
                    connection = Server.Accept();
                }
                catch (Exception ex)
                {   
                    Console.WriteLine(ex.Message);
                    break;
                }
                    connections.Add(connection.RemoteEndPoint.ToString(), connection);
                    Console.WriteLine("客户端连接：[" + connection.RemoteEndPoint + "]客户端总数：" + connections.Count);
                    IPAddress RemoteClientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                    int Port = (connection.RemoteEndPoint as IPEndPoint).Port;
                    connection.Send(Encoding.ASCII.GetBytes("客户端[" + RemoteClientIP + ":" + Port.ToString() + "]连接服务器成功！"));
                Thread revthread = new Thread(rev);
                revthread.Start(connection);
            }
        }
        private static void rev(object socketclient)
        {
            Socket socketServer = socketclient as Socket;
            while (true)
            {
                try
                {
                    byte[] Cache = new byte[1024 * 1024];
                    int length = socketServer.Receive(Cache);
                    string text = Encoding.ASCII.GetString(Cache, 0, length);
                    if (connections.Count > 0)
                    {
                        foreach (var client in connections)
                        {
                            client.Value.Send(Encoding.ASCII.GetBytes(text));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.ReadLine();
                    break;
                }
            }
        }
    }
}
    //class Program
    //{
    //    static int UsersInt;
    //    static string[] UsersCache=new string[9];
    //    static Socket socket;
    //    static int i = -1;
    //    static Socket[] sockets = new Socket[9];
    //    static void Main(string[] args)
    //    {
    //        IPAddress IP = IPAddress.Parse("127.0.0.1");
    //        UsersInt = 0;
    //        int port = 25565;
    //        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
    //        socket.Bind(new IPEndPoint(IP, port));
    //        Thread thread = new Thread(Connect);
    //        thread.Start();
    //    }
    //    private static void Connect()
    //    {
    //        socket.Listen(10);
    //        while (true)
    //        {
    //            byte[] res = new byte[1024*1024];
    //            Socket clientsocket1 = socket.Accept();
    //            UsersInt += 1;
    //            Console.WriteLine("New:" + clientsocket1.RemoteEndPoint);
    //            UsersCache[UsersInt - 1] = clientsocket1.RemoteEndPoint.ToString();
    //            int length=clientsocket1.Receive(res);
    //            string str = Encoding.ASCII.GetString(res);
    //            Regex reg=new Regex("l");
    //            Match mat = reg.Match(str);
    //            if (mat.Index != 0)
    //            {
    //                string str2 = str.Replace("hl","");
    //                string[] sArray = str2.Split(':');//分割后的文本
    //                Console.WriteLine("dlzh" +sArray[0]);
    //                Console.WriteLine("dzmm" + sArray[1]);
    //            }else if (mat.Index == 0)
    //            {
    //                Regex reg1 = new Regex("r");
    //                Match mat1 = reg1.Match(str);
    //                if (mat1.Index != 0)
    //                {
    //                    string str3 = str.Replace("hr", "");
    //                    string[] sArray2 = str3.Split(':');
    //                    Console.WriteLine("rzh" +sArray2[0]);
    //                    Console.WriteLine("rmm"+sArray2[1]);
    //                }else if (mat1.Index == 0)
    //                {
    //                    clientsocket1.Send(res);
    //                }
    //            }
    //        }
    //    }
    //}
