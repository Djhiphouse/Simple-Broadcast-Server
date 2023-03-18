```cs 
Useage:
Server TCPServer = new Server("127.0.0.1", 8080);
Task.Run(TCPServer.HandleConnection);
```

```cs
Adding Broadcast:
while (true)
			{
				Thread.Sleep(2000);
				Console.WriteLine("Broadcasted!");
				Task.Run(() => { TCPServer.Broadcast("Broadcast Message"); });
			} 
```
