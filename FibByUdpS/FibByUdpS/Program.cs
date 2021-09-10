using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace FibByUdpS
{
    class Program
    {
        static void Send(long value, ref int seq, Socket sock,EndPoint addr)
        {
            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(seq);
                writer.Write(value);
            }
            byte[] dataToSend = stream.ToArray();

            sock.SendTo(dataToSend, addr);
            Console.WriteLine($"Sended { value} to { addr}");
            seq += 1;
        }
        static int Recive(Socket sock, ref int lastSeq, ref EndPoint addr)
        {
            int value = 0;
            byte[] data = new byte[8];
            //EndPoint addr = new IPEndPoint(0, 0); // параметры будут заполнены позже
            sock.ReceiveFrom(data, ref addr);
            MemoryStream stream = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int seq = reader.ReadInt32();
                value = reader.ReadInt32();
                if (seq > lastSeq)
                {
                    Console.WriteLine($"Received { value} from { addr}");
                    lastSeq = seq;
                }
            }
            return value;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            Socket sock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
                );
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint addr = new IPEndPoint(ip, 1337);
            //sock.Bind(addr);
            
            int seq = 1;
            Send(1, ref seq, sock, addrs);
            int lastSeq = 0;
            long value = 1;
            EndPoint addrs = new IPEndPoint(0, 0);
            int valueR = Recive(sock, ref lastSeq, ref addrs);
            Console.WriteLine(addrs);

            for (int i = 1; i <= valueR; i++)
            {
                value = value * i;
                if (value == 0)
                {
                    Console.WriteLine("?!");
                }
            }
            Send(value, ref seq, sock, addrs);
            

        }
    }
}
