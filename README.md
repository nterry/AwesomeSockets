AwesomeSockets
=========


[![Build status](https://ci.appveyor.com/api/projects/status/smh77tfj8rbqsiuy)](https://ci.appveyor.com/project/nterry/awesomesockets)&nbsp;&nbsp;&nbsp;&nbsp;[![Build Status](https://travis-ci.org/nterry/AwesomeSockets.svg?branch=master)](https://travis-ci.org/nterry/AwesomeSockets)&nbsp;&nbsp;&nbsp;&nbsp;[![NuGet version](https://badge.fury.io/nu/AwesomeSockets.png)](http://badge.fury.io/nu/AwesomeSockets)


What is AwesomeSockets?
-----------------------

AwesomeSockets is a C# library that facilitates network communication. It fully supports both 
synchronous and asynchronous communication. Callbacks for asynchronous calls can be provided 
as either a delegate or as a lambda.
	
Documentation
-------------
	
Using AwesomeSockets is very easy! There are two main object types, the `Buffer` and the `Socket`. The `Buffer` is a vehicle to hold the data, and the `Socket` is responsible for sending/receiving the data. All the major public methods are static, meaning you call them on the class like so:

	
	Buffer inBuf = Buffer.new();
	
In order to send a message, you must first connect to them:

	//Server-side code
	ISocket listenSocket = AweSock.TcpListen(14804);
	ISocket client = AweSock.TcpAccept(listenSocket);
	
	//Client-side code
	ISocket server = AweSock.TcpConnect(1.2.3.4, 14804);
	
Once you've connected, you need to construct and populate at least two `Buffer` objects; one for receiving and one for sending:

	Buffer inBuf = Buffer.New();
	Buffer outBuf = Buffer.New();
	
	//Lets send some data to the server! Make a Buffer object like so
	Buffer.ClearBuffer(outBuf);
	Buffer.Add(outBuf, 42);
	Buffer.Add(outBuf, "Is the ultimate answer");
	Buffer.Add(outBuf, 'N');
	Buffer.FinalizeBuffer(outBuf);
	
	//Now lets send it to the server!
	int bytesSent = AweSock.SendMessage(server, outBuf);
	
	//And receive any inbound messages as well
	Tuple<int, EndPoint> received = AweSock.ReceiveMessage(server, inBuf);

	
And thats it! The TcpAccept method will block until a connection comes in and returns the the connected `Socket`. If you want to have it non-blocking, we support that as well:
	
	//Non-blocking mode returns null
	AweSock.TcpAccept("", 12, SocketCommunicationTypes.NonBlocking, (listenSocket, error) => { return null; });
		
As you can see, we provided the NonBlocking constant and a lambda as a callback when the connection is accepted. If the listensocket parameter above is null, the error field will have an exception, indicating failure. Conversely, if the listensocket isn't null, the error will be, indicating success. This same pattern applies to TcpConnect as well.

Udp is similar, but with one major difference; the connect logic is 'reversed' for the server side. Here is an example:

	//Server-side code (5.6.7.8 is the ip of the client)
	ISocket client = AweSock.UdpConnect('5.6.7.8', 14804);
	
	//Server-side code (1.2.3.4 is the ip of the server)
	ISocket server = AweSock.UdpConnect('1.2.3.4', 14804);
	
Both ends must know the ip of the other player before hand. This is where Tcp can come in. You can establish an ephermeral Tcp socket and use the ISocket object to get each others Ip addresses. It is also worth noting, there is no ability to have blocking logic as Udp is stateless. Everything else is identical in usage to Tcp.

If you have any additional questions, shoot me an email at nick.i.terry@gmail.com or message me on github @nterry. Additional documentation will be added as XML as needed.
