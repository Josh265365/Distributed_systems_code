using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DistSysAcwServer.Models;
using Newtonsoft.Json;



#region Task 10 and beyond

#endregion
class Progam
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Hello. What would you like to do?");
            Console.Write("Type your command: ");
            string userInput = Console.ReadLine();

            if (userInput.ToLower() == "exit")
                break;

            await ProcessCommand(userInput);
        }
    }

    static async Task ProcessCommand(string userInput)
    {
        using (var client = new HttpClient())
        {
            string[] parts = userInput.Split(' ');
            string action = parts[0].ToLower(); // First part is the action

            switch (action)
            {
                case "talkback":
                    await HandleTalkBackCommand(client, parts);
                    break;
                case "user":
                    await HandleUserCommand(client, parts);
                    break;
                case "protected":
                    await HandleProtectedommand(client, parts);
                    break;
                 // Add cases for other commands (user, protected) as needed
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    static async Task HandleTalkBackCommand(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid talkback command.");
            return;
        }

        string subAction = parts[1].ToLower();
        switch (subAction)
        {
            case "hello":
                await HandleTalkBackHello(client);
                break;
            case "sort":
                await HandleTalkBackSort(client, parts);
                break;
            default:
                Console.WriteLine("Invalid talkback subcommand.");
                break;
        }
    }

    static async Task HandleUserCommand(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid user command.");
            return;
        }

        string subAction = parts[1].ToLower();
        switch (subAction)
        {
            case "get":
                await HandleUserGet(client, parts);
                break;
            case "post":
                await HandleUserPost(client, parts);
                break;
            case "set":
                await HandleUserSet(client, parts);
                break;
            case "delete":
                await HandleUserDelete(client, parts);
                break;
            case "role":
                await HandleUserRole(client, parts);
                break;
            default:
                Console.WriteLine("Invalid user subcommand.");
                break;
        }
    }

  static async Task HandleProtectedommand(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid protected command.");
            return;
        }

        string subAction = parts[1].ToLower();
        switch (subAction)
        {
            case "hello":
                await HandleProtectedHello(client);
                break;
            case "sha1":
                await HandleProtectedSha1(client, parts);
                break;
            case "sha256":
                await HandleProtectedSha256(client, parts);
                break;
            case "get publickey":
               // await HandleProtectedGetPublicKey(client);
                break;
            case "sign hello":
              //  await HandleProtectedSign(client, parts);
                break;
            case "mashififty":
               // await HandleProtectedAddFifty(client, parts);
                break;
            default:
                Console.WriteLine("Invalid protected subcommand.");
                break;
        }

    }

    private static async Task HandleProtectedSha256(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 3)
        {
            Console.WriteLine("Invalid sha256 command. Usage: protected sha256 <message>");
            return;
        }

        // Get the locally stored API key
        string storedapiKey = User.GetStoredApiKey();

        // Check if the API key exists
        if (storedapiKey == null)
        {
            Console.WriteLine("You need to do a User Post or User Set first");
            return;
        }

        string message = parts[2];

        Console.WriteLine(".............Please wait..........");

        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:44394/api/protected/sha256?message={message}"),
            };

            // Add the API key to the request headers
            request.Headers.Add("ApiKey", storedapiKey);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
       
    }

    private static async Task HandleProtectedSha1(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 3)
        {
            Console.WriteLine("Invalid sha1 command. Usage: protected sha1 <message>");
            return;
        }

        // Get the locally stored API key
        string storedapiKey = User.GetStoredApiKey();

        // Check if the API key exists
        if (storedapiKey == null)
        {
            Console.WriteLine("You need to do a User Post or User Set first");
            return;
        }

        string message = parts[2];

        Console.WriteLine(".............Please wait..........");

        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:44394/api/protected/sha1?message={message}"),
            };

            // Add the API key to the request headers
            request.Headers.Add("ApiKey", storedapiKey);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
    }


    private static async Task HandleProtectedHello(HttpClient client)
    {
        Console.WriteLine(".............Please wait..........");

        try
        {
            var storedApiKey = User.GetStoredApiKey();
            if (storedApiKey == null)
            {
                Console.WriteLine("You need to do a User Post or User Set first");
                return;
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://localhost:44394/api/protected/hello"),
            };
            request.Headers.Add("ApiKey", storedApiKey);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
       
    }

    private static async Task HandleUserRole(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 4)
        {
            Console.WriteLine("Invalid user role command. Usage: user role <username> <role>");
            return;
        }

        string inputUsername = parts[2];
        string inputRole = parts[3];

        string storedApiKey = User.GetStoredApiKey();

        // Check if the stored API key exists
        if (string.IsNullOrEmpty(storedApiKey))
        {
            Console.WriteLine("You need to do a User Post or User Set first");
            return;
        }

        // Validate the input role
        if (inputRole != "User" && inputRole != "Admin")
        {
            Console.WriteLine("Invalid role. Role must be either 'User' or 'Admin'.");
            return;
        }

        try
        {
            var requestBody = new ChangeUserRole
            {
                UserName = inputUsername,
                NewRole = inputRole
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,//change to put
                RequestUri = new Uri($"https://localhost:44394/api/user/ChangeRole"),
                Headers = {
                { "ApiKey", storedApiKey }
            },
                Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent == "DONE")
                {
                    Console.WriteLine("Role changed successfully");
                }
                else
                {
                    Console.WriteLine("An error occurred while changing the role.");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (errorContent.Contains("NOT DONE: Username does not exist."))
                {
                    Console.WriteLine($"Error: User '{inputUsername}' does not exist.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                }
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
    }

    private static async Task HandleUserDelete(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid user delete command.");
            return;
        }

        string storedUsername = User.GetStoredUsername();
        string storedApiKey = User.GetStoredApiKey();


        // Check if the stored username and API key exist
        if (storedUsername == null || storedApiKey == null)
        {
            Console.WriteLine("You need to do a User Post or User Set first");
            return;
        }

        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://localhost:44394/api/user/removeuser?username={storedUsername}"),
            };
            request.Headers.Add("ApiKey", storedApiKey);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                //  if (responseContent.Contains("True"))
                // {
                //  Console.WriteLine("True");
                // }
                //  else
                //{
                // Console.WriteLine("False");

                // }

                bool deleteSuccessful = bool.Parse(responseContent);
                Console.WriteLine(deleteSuccessful);
            }
            else
            {
                Console.WriteLine($"Error: not deleted {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
    }


    private static async Task HandleUserSet(HttpClient client, string[] parts)
    {

        // Check if the command is valid
        if (parts.Length < 4)
        {
            Console.WriteLine("Invalid user set command. Please provide a username and API key.");
            return;
        }

        string username = parts[2];
        string apiKey = parts[3];

        // Store the username and API key as local variables
       // string storedUsername = username;
       // string storedApiKey = apiKey;
       User.setUser(username, apiKey);


       Console.WriteLine("Stored username: " + username +" and apiKey: " + apiKey);

    }



    private static async Task HandleUserPost(HttpClient client, string[] parts)
    {
        // Check if the command is valid
        if (parts.Length < 3)
        {
            Console.WriteLine("Invalid user post command. Please provide a username.");
            return;
        }

        string username = parts[2];

        Console.WriteLine(".............Please wait..........");

        try
        {
           
            var content = new StringContent($"\"{username}\"", Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:44394/api/user/new", content);

            if (response.IsSuccessStatusCode)
            {
               // var responseContent = await response.Content.ReadAsStringAsync();
                var apiKey = await response.Content.ReadAsStringAsync();
               // if (responseContent.Equals("OK"))
               // {
                   // var apiKey = response.Headers.GetValues("API-Key").FirstOrDefault();
                    //  Console.WriteLine($"Got API Key: {apiKey}");
                    // Console.WriteLine($"Username: {username}");
                  User.setUser(username, apiKey);
                Console.Clear();
                Console.WriteLine("Got Api-Key");

                
                
               // }
               // else
               // {
                   // Console.WriteLine(responseContent);
              //  }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("Error: failed to send request");
        }
    }


    private static async Task HandleUserGet(HttpClient client, string[] parts)
    {
        //// Check if the command is valid
        if (parts.Length < 3)
        {
            Console.WriteLine("Invalid user get command.");
            return;
        }


        //string apiKey = parts[3];

        Console.WriteLine(".............Please wait..........");
        string username = parts[2];

        try
        {
            var response = await client.GetAsync($"https://localhost:44394/api/user/new?username={username}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine($"Error: failed to send request");
        }

    }

    static async Task HandleTalkBackHello(HttpClient client)
    {
        Console.WriteLine(".............Please wait..........");

        try
        {
            var response = await client.GetAsync("https://localhost:44394/api/talkback/hello");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine($"Error: failed to send request");
        }
    }

    static async Task HandleTalkBackSort(HttpClient client, string[] parts)// pass  interger array with spaces e.g 1 2 3 4
    {
        // Check if the command is valid
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid talkback sort command.");
            return;
        }

        Console.WriteLine(".............Please wait..........");

        try
        {
            // Extract integers from input
            List<int> integers = new List<int>();
            for (int i = 2; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out int num))
                {
                    integers.Add(num);
                }
                else
                {
                    Console.WriteLine("Invalid integer: " + parts[i]);
                    return;
                }
            }

            // Create query string for API request
            string queryString = string.Join("&integers=", integers);

            var response = await client.GetAsync($"https://localhost:44394/api/talkback/sort?integers={queryString}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine($"Error: failed to send request");
        }
    }



}




