// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDbDataAccess.DataAccess;
using MongoDbDataAccess.Models;
using MongoDbDemo;

//var client = new MongoClient(connectionString);
//var db = client.GetDatabase(databaseName);
//var collection = db.GetCollection<PersonModel>(collectionName);

//var person = new PersonModel 
//{ Name = "Iqra Farooqi",
//    Address = "Wafaqi Colony",
//    Degrees = new Degree[] { new Degree { DegreeName = "BS" }, new Degree { DegreeName = "MS" } }
//};

////collection.InsertOne(person);

//var results = collection.Find(_ => true);

//foreach (var item in results.ToList())
//{
//    Console.WriteLine($"{item._id} , {item.Name} lives in {item.Address} ");
//}

ChoreDataAccess db = new ChoreDataAccess();
await db.CreateUser(new UserModel { FirstName = "Imran", LastName = "Mani" });

var users = await db.GetAllUsers();

var chore = new ChoreModel
{
    AssignedTo = users.First(),
    ChoreText = "Mow the lawn",
    FrequencyInDays = 7,
};

await db.CreateChore(chore);


var chores = await db.GetAllChores();

await db.CompleteChore(chores.First());