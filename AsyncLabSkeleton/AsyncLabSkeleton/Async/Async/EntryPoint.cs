using Client;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;



// Add code here to send requests

//ClientBehaviours.SendRequest("Hello World","TaskOne");
//ClientBehaviours.SendRequest("Hello World", "TaskTwo");
//ClientBehaviours.SendRequest("Hello World", "TaskThree");

Task one = ClientBehaviours.SendRequest("Hello World", "TaskOne");
Task two = ClientBehaviours.SendRequest("Hello World", "TaskTwo");
Task three = ClientBehaviours.SendRequest("Hello World", "TaskThree");

Stopwatch sw = new Stopwatch();
one.Wait();

Console.WriteLine("Execution finished");
Console.ReadLine();