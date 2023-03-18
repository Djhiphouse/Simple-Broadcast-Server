Server TCPServer = new Server("127.0.0.1", 8080);
Task.Run(TCPServer.HandleConnection);
