namespace MyFirstAPI.DataAccess
{
    public class MyDataCRUD
    {
       
       
        private string[] MyData { get; set; }
        public void Create()
        {
            MyData = new string[] { "zero", "one", "two", "three", "four", "five" };
        }
        public string[] Read()
        {
            return MyData;
        }
        public string Read(int index)
        {
            return MyData[index];
        }
        public void Update(int index, string data)
        {
            MyData[index] = data;
        }
        public void Delete(int index)
        {
            MyData[index] = "Deleted";
        }



    }
}
