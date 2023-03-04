// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDbDemo;


namespace MongoDbConsole
{
    public class Program
    {
        private static string connectionString = "mongodb+srv://imran89pieas:asdfasdfasdf@cluster0.orcsqfi.mongodb.net/test";
        private static string databaseName = "MyDatabase";
        private static string collectionName = "MyCollection";



        public static void Main(string[] args)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            var collection = db.GetCollection<PersonModel>(collectionName);

            var person = new PersonModel 
            { Name = "Iqra Farooqi",
                Address = "Wafaqi Colony",
                Degrees = new Degree[] { new Degree { DegreeName = "BS" }, new Degree { DegreeName = "MS" } }
            };

            //collection.InsertOne(person);
            
            var results = collection.Find(_ => true);

            foreach (var item in results.ToList())
            {
                Console.WriteLine($"{item._id} , {item.Name} lives in {item.Address} ");
            }

        }
    }

}

