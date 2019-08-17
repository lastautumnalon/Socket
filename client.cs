using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketClient
{
    class Program
    {
        static Socket Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static IPAddress IP;
        static int Port;
        static void Main(string[] args)
        {
            IP = IPAddress.Parse(Console.ReadLine());
            Port = int.Parse(Console.ReadLine());
            Client.Connect(new IPEndPoint(IP,Port));
            Thread rs = new Thread(ReceiveSend);
            rs.Start();
        }
        private static void ReceiveSend()
        {
            Thread rec = new Thread(Rec);
            rec.Start();
            while (true)
            {
                string msg = Console.ReadLine();
                Client.Send(Encoding.ASCII.GetBytes(msg));
            }
        }
        private static void Rec()
        {
            while (true)
            {
                byte[] Cache = new byte[1024 * 1024];
                int length = Client.Receive(Cache);
                string text = Encoding.ASCII.GetString(Cache, 0, length);
                Console.WriteLine(text);
            }
        }
    }
    //class Program
    //{
    //    static string zh;
    //    static string mm;
    //    static string zczh;
    //    static string zcmm;
    //    static Socket clientSocket;
    //    static string name;
    //    static int lor;
    //    static void Main(string[] args)
    //    {
    //        IPAddress IP = IPAddress.Parse("127.0.0.1");
    //        lor = 0;
    //        int port = 25565;
    //        clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
    //        try
    //        {
    //            clientSocket.Connect(new IPEndPoint(IP, port));
    //            Console.WriteLine("连接成功！");
    //            Console.WriteLine("登录请输入l,注册请输入reg");
    //            string dr = Console.ReadLine();
    //            if (dr == "l")
    //            {
    //                Console.WriteLine("输入账号");
    //                zh = Console.ReadLine();
    //                Console.WriteLine("密码");
    //                mm = Console.ReadLine();
    //                lor = 1;
    //                name = zh;
    //            }else if (dr == "reg")
    //            {
    //                Console.WriteLine("输入账号");
    //                zczh = Console.ReadLine();
    //                Console.WriteLine("密码");
    //                zcmm = Console.ReadLine();
    //                lor = 2;
    //                name = zczh;
    //            }
    //            Thread Upload = new Thread(UploadUsernameandPass);
    //            Upload.Start();
    //        }
    //        catch(Exception ex)
    //        {
    //            Console.WriteLine("Error"+ex);
    //            Console.ReadLine();
    //        }
    //    }
    //    private static void UploadUsernameandPass()
    //    {
    //        if (lor == 1)
    //        {
    //            clientSocket.Send(Encoding.ASCII.GetBytes("hl"+zh+":"+mm));
    //        }else if (lor == 2)
    //        {
    //            clientSocket.Send(Encoding.ASCII.GetBytes("hr"+zczh+":"+zcmm));
    //        }
    //        Thread Mainthread = new Thread(MainThread);
    //        Mainthread.Start();
    //    }
    //    private static void MainThread()
    //    {
    //        while (true)
    //        {
    //            string Message = Console.ReadLine();
    //            clientSocket.Send(Encoding.ASCII.GetBytes(name+":"+Message));
    //        }
    //    }
    //    private static void ReveiveThread()
    //    {
    //        string receiveMsg="";
    //        byte[] Cache = new byte[1000];
    //        int length = clientSocket.Receive(Cache);
    //        receiveMsg = Encoding.ASCII.GetString(Cache,0,length);
    //        Console.WriteLine(receiveMsg); 
    //    }
    //}
}