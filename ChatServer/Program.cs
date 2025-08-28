using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    const string ipAddressString = "127.0.0.1";
    const int port = 8000;

    static void  Main()
    {
        IPAddress ipAddress = IPAddress.Parse(ipAddressString);
        TcpListener server = new TcpListener(ipAddress, port);
        server.Start();
        Console.WriteLine($"Server started.");
        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            Task.Run(() => ClientJoined(client));
        }
    }

    static void ClientJoined(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[8064];
        int bytesNameRead = stream.Read(buffer, 0, buffer.Length);
        string userName = Encoding.UTF8.GetString(buffer, 0, bytesNameRead);
        Console.WriteLine($"{userName} has connected to the server.");
        
        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine(message);
            byte[] replyData = Encoding.UTF8.GetBytes(message);
            stream.Write(replyData, 0, replyData.Length);

        }
    }
}