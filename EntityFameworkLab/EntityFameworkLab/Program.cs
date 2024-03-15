// See https://aka.ms/new-console-template for more information
using EntityFameworkLab;

using (var ctx = new MyContext())
{
    Address addr = new Address()
    {
        House_Name_or_Number = "1076",
        Street = "Some Street",
        City = "Some City",
        County = "Some County",
        Postcode = "Some Postcode",
        People = new List<Person>(),
        Country = "UK"
    };

    Person prsn = new Person()
    {
        First_Name = "Jane",
        Middle_Name = "Janet",
        Last_Name = "Doe",
        Date_of_Birth = new DateTime(2010, 10, 1),
        Age = 40,
        Address = addr
    };

    ctx.Addresses.Add(addr);
    ctx.People.Add(prsn);
    ctx.SaveChanges();
}