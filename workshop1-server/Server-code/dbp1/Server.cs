using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5002);
tcpListener.Start();
TcpClient tcpClient = tcpListener.AcceptTcpClient(); NetworkStream nStream = tcpClient.GetStream();
string message = ReadFromStream(nStream);
Console.WriteLine("Received: \"" + message + "\"");
string translatedMessage = Translate(message);
// TODO: Serialize the translated message and write it to the stream
Console.WriteLine("translated: \"" + translatedMessage + "\"");

Console.ReadKey(); // Wait for keypress before exit
static string Translate(string message)
{
    string translatedmessage = " ";
    string[] words = message.Split(' ');
    foreach (string word in words)
    {
        // TODO: Perform translation

        if (IsVowel(word[0]))
        {
            translatedmessage += word + "way ";
        }
        else if (IsConsonant(word[0]))
        {
            if (IsConsonant(word[1]))
            {
                translatedmessage += word.Substring(2) + word.Substring(0, 2) + "ay ";
            }
            else
            {
                translatedmessage += word.Substring(1) + word[0] + "ay ";
            }
        }
    }
    return translatedmessage.Trim();
}

static bool IsVowel(char c)
{
    return "aeiouAEIOU".IndexOf(c) != -1;
}

static bool IsConsonant(char c)
{
    return !IsVowel(c);
}
static string ReadFromStream(NetworkStream stream)
{
    byte[] messageLengthBytes = new byte[1];
    stream.Read(messageLengthBytes, 0, 1);
    byte[] messageBytes = new byte[messageLengthBytes[0]];
    stream.Read(messageBytes, 0, messageLengthBytes[0]);
    return Encoding.ASCII.GetString(messageBytes);
}
