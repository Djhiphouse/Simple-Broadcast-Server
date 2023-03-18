using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Broadcast_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Bind on Target like Localhost
			Server TCPServer = new Server("127.0.0.1", 9090);
			//Start Handle TcpClients async
			Task.Run(TCPServer.HandleConnection);

			//Broadcast every 2 seconds 1 Test Message aysnc
			while (true)
			{
				Thread.Sleep(2000);
				Console.WriteLine("Broadcasted!");
				Task.Run(() => { TCPServer.Broadcast("test Message"); });
			}

		}
	}
	public class Server
	{
		public string Serverip { get; set; }
		public int Serverport { get; set; }

		public List<TcpClient> ConnectedClients = new List<TcpClient>();
		public Server(string IP, int port)
		{
			Serverip = IP;
			Serverport = port;
		}

		public async Task HandleConnection()
		{
			//Start Listening here
			TcpListener tcp = new TcpListener(IPAddress.Parse(Serverip), Serverport);
			tcp.Start();
			//add incomeing Client to Array of Connected Clients
			while (true)
			{
				TcpClient client = tcp.AcceptTcpClient();
				Console.WriteLine("New Client Connected! - Count: " + ConnectedClients.Count);
				ConnectedClients.Add(client);
			}


		}

		//Broadcast Methode
		public async Task Broadcast(String Message)
		{
			//Loop through TcpClient list
			foreach (var client in ConnectedClients)
			{
				//Check for alive Connections
				if (!client.Connected)
				{
					ConnectedClients.Remove(client);
					Console.WriteLine("Removeing dead client!");
					return;
				}

				//Broadcast to all Connected Clients
				await client.GetStream().WriteAsync(Encoding.UTF8.GetBytes(Message), 0, Message.Length);
			}

		}
	}
}
