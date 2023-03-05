using MongoDbDataAccess.Models;
using MongoDB.Driver;
namespace MongoDbDataAccess.DataAccess;
public class ChoreDataAccess
{
    private const string ConnectionString = "*Use Your Own Database here*";
    private const string DatabaseName = "Choredb";
    private const string ChoreCollection = "chore_chart";
    private const string UserCollection = "users";
    private const string ChoreHistoryCollection = "chore_history";


    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection);
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        var result = await usersCollection.FindAsync<UserModel>(_ => true);

        return result.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChores()
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var result = await choresCollection.FindAsync(_ => true);

        return result.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChoresForAUser(UserModel user)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var result = await choresCollection.FindAsync(c => c.AssignedTo._id == user._id );

        return result.ToList();
    }

    public Task CreateUser(UserModel user)
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        return usersCollection.InsertOneAsync(user);
    }


    public Task CreateChore(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        return choresCollection.InsertOneAsync(chore);
    }
    
    public Task UpdateChore(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var filter = Builders<ChoreModel>.Filter.Eq("_id", chore._id );

        return choresCollection.ReplaceOneAsync(filter, chore, options: new ReplaceOptions { IsUpsert = true});
    }

    public Task DeleteChore(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        return choresCollection.DeleteOneAsync(c => c._id == chore._id);
    }

    public async Task CompleteChore(ChoreModel chore)
    {
        //var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        //var filter = Builders<ChoreModel>.Filter.Eq(field: "_id", chore._id);
        //await choresCollection.ReplaceOneAsync(filter, chore);

        //var choreHistoryCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistoryCollection);
        //await choreHistoryCollection.InsertOneAsync(document: new ChoreHistoryModel(chore));

        var client = new MongoClient(ConnectionString);
        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(DatabaseName);
            var choresCollection = db.GetCollection<ChoreModel>(ChoreCollection);
            var filter = Builders<ChoreModel>.Filter.Eq(field: "_id", chore._id);
            chore.LastCompleted = DateTime.Now;
            await choresCollection.ReplaceOneAsync(filter, chore);
            var choreHistoryCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistoryCollection);
            await choreHistoryCollection.InsertOneAsync(document: new ChoreHistoryModel(chore));


            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            
        }

    }
}
