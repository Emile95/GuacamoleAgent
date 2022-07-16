using System.Net;
using System.Net.Sockets;

namespace GuacamoleClient.Sockets
{
    public class ServerSocket
    {
        private readonly int _port;
        private readonly Socket _socket;

        public ServerSocket(int port)
        {
            _port = port;
            _socket = new Socket(
                Dns.GetHostAddresses(Dns.GetHostName())[0].AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Start()
        {
            _socket.Connect(new IPEndPoint(Dns.GetHostAddresses(Dns.GetHostName())[0], 1100));

            StateObject state = new StateObject();
            state.workSocket = _socket;

            _socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallBack), state);
        }

        private void ReadCallBack(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket socket = state.workSocket;

            int bytesRead = 0;
            try
            {
                bytesRead = socket.EndReceive(ar);
            }
            catch (Exception e)
            {
                return;
            }

            socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallBack), state);
        }
    }
}
