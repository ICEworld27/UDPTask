using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FibByUdp
{
    class Program
    {
        static void Send(int value, int seq, Socket sock)
        {
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(seq);
                writer.Write(value);
            }
            byte[] dataToSend = stream.ToArray();

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint addr = new IPEndPoint(ip, 1337);
            sock.SendTo(dataToSend, addr);
            seq += 1;
        }
        static long Recive(Socket sock, int lastSeq)
        {
            long value = 0;
            byte[] data = new byte[12];
            //List<byte> data = new List<byte>();
            EndPoint addr = new IPEndPoint(0, 0); // параметры будут заполнены позже
            sock.ReceiveFrom(data, ref addr);
            MemoryStream stream = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int seq = reader.ReadInt32();
                value = reader.ReadInt64();
                if (seq > lastSeq)
                {
                    Console.WriteLine($"Received { value} from { addr}");
                    lastSeq = seq;
                }
            }
            return value;
        }

        static void Main(string[] args)//
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("Kill pc if error occurred? (Y/N)");
            string readed = Console.ReadLine();
            bool kill = false;
            if (readed == "Y" || readed == "y")
            {
                kill = true;
            }
            Console.WriteLine("Enter value:");
            int value = Convert.ToInt32(Console.ReadLine());
            Socket sock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
                );
            int seq = 1;
            int lastSeq = 0;
            string[] eror = { "!", ",", "|", "/", "@", "#", "$", "%", "^", "&", "*", "(", ")", "<", ">", "?" };

            if (value > 20)
            {
                Console.WriteLine("Слишком большое число...");
                Thread.Sleep(2000);
                Console.WriteLine("Erоr");
                Thread.Sleep(1000);
                Console.Clear();
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < eror.Length; i++)
                    {
                        int posX = Console.CursorLeft, posY = Console.CursorTop;
                        Console.WriteLine(eror[i]);
                        Thread.Sleep(250);
                        Console.SetCursorPosition(posX, posY);
                        for (int j = 0; j < eror[i].Length; j++)
                        {
                            Console.Write("");
                        }
                        Console.SetCursorPosition(posX, posY);

                    }
                    Thread.Sleep(1000);
                }
                Thread.Sleep(2000);
                if (kill)
                {
                    System.Diagnostics.Process.Start("shutdown", "/s /t /f 00");
                }
                Environment.Exit(0);
            }
            Send(value, seq, sock);
            Console.WriteLine("Sended!");
            Console.WriteLine(Recive(sock, lastSeq));
            
            
            
        }
    }
}
