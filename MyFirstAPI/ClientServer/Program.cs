
HttpClient client = new HttpClient();
RunAsync(client).Wait();
Console.ReadKey();
static async Task RunAsync(HttpClient client)
{
    client.BaseAddress = new Uri("http://localhost:12311/api/");
    try
    {
        Task<string> task = GetStringAsync("Data/Name?name=John", client);
        if (await Task.WhenAny(task, Task.Delay(20000)) == task)
        {
            Console.WriteLine(task.Result);
        }
        else
        {
            Console.WriteLine("Request Timed Out");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetBaseException().Message);
    }
}
static async Task<string> GetStringAsync(string path, HttpClient client)
{
    string responsestring = "";
    HttpResponseMessage response = await client.GetAsync(path);
    responsestring = await response.Content.ReadAsStringAsync();
    return responsestring;
}