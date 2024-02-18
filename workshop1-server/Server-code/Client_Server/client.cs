using System.Net.Sockets;
using System.Text;

TcpClient tcpClient = new TcpClient();
tcpClient.Connect("127.0.0.1", 5002);
NetworkStream nStream = tcpClient.GetStream();
Console.WriteLine("Enter a message to be translated...");
string message = Console.ReadLine();
byte[] request = Serialize(message);
nStream.Write(request, 0, request.Length);
// TODO: Read response from stream and display to user
Console.ReadKey(); // Wait for keypress before exit
byte[] Serialize(string request)
{
    byte[] responseBytes = Encoding.ASCII.GetBytes(request);
    byte responseLength = (byte)responseBytes.Length;
    byte[] responseLengthBytes = BitConverter.GetBytes(responseLength);
    byte[] rawData = new byte[responseLength + 1];
    responseLengthBytes.CopyTo(rawData, 0);
    responseBytes.CopyTo(rawData, 1);
    return rawData;
}
