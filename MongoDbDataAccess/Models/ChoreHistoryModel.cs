
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDataAccess.Models;
public class ChoreHistoryModel
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string _id { get; set; }
    public string ChoreId { get; set; }
    public string ChoreText { get; set; }
    public int FrequencyInDays { get; set; }
    public UserModel? WhoCompleted { get; set; }
    public DateTime? DateCompleted { get; set; }

    public ChoreHistoryModel()
    {

    }

    public ChoreHistoryModel(ChoreModel model)
    {
        ChoreId = model._id;
        DateCompleted = model.LastCompleted;
        WhoCompleted = model.AssignedTo;
        ChoreText = model.ChoreText;
    }


}
