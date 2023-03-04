using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDemo;


public class PersonModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public Degree[]? Degrees { get; set;}
}

public class Degree
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string DegreeName { get; set; }
}
