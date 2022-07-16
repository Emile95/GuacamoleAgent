using GuacamoleClient.Sockets;

ServerSocket serverSocket = new ServerSocket(1100);

serverSocket.Start();

string? line;
do
{
    line = Console.ReadLine();
}
while (line != "quit");