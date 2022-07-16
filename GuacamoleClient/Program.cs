using GuacamoleClient;
using GuacamoleClient.Sockets;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

/*Socket socket = new Socket(
    Dns.GetHostAddresses(Dns.GetHostName())[0].AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp
);

socket.Bind(new IPEndPoint(Dns.GetHostName()[0], 1002));
socket.Listen(100);

socket.BeginAccept((asy) =>
{
    Socket listener = (Socket)asy.AsyncState;
    Socket handler = listener.EndAccept(asy);

    Console.WriteLine("Server Connected");

}, socket);*/

Socket serverSocket = new Socket(
    Dns.GetHostAddresses(Dns.GetHostName())[0].AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp
);

serverSocket.Connect(new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], 1100));

StateObject state = new StateObject();
state.workSocket = serverSocket;

serverSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, (asr) => {
    StateObject state = (StateObject)asr.AsyncState;
    Socket clientSocket = state.workSocket;

    string data = System.Text.Encoding.ASCII.GetString(state.buffer);
    Console.Write(JsonConvert.DeserializeObject<SocketData>(data).Count);
}, state);

SocketData socketData = new SocketData {
    Count = 157
};

JsonSerializerSettings settings = new JsonSerializerSettings
{
    TypeNameHandling = TypeNameHandling.All
};
serverSocket.Send(System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(socketData, settings)));

Console.ReadLine();
