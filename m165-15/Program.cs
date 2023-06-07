using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


Console.WriteLine("Hello, World!");

string connectionString = "mongodb://localhost:27017";
var client = new MongoClient(connectionString);

var databases = client.ListDatabases().ToList();
Console.WriteLine("Available Databases:");
foreach (var database in databases)
{
    Console.WriteLine("DB Name: {0}", database.GetValue("name"));
    var dbName = database.GetValue("name").ToString();
    var db = client.GetDatabase(dbName);

    Console.WriteLine("Collections:");
    var collections = db.ListCollectionNames().ToList();
    foreach (var collection in collections)
    {
        Console.WriteLine(collection);
    }
    Console.WriteLine("------------------------------");
}

// Aufgabe 1
var movieDatabase = client.GetDatabase("M165");
var moviesCollection = movieDatabase.GetCollection<Movie>("Movies");

var query = moviesCollection.AsQueryable()
            .Where(r => r.Year == 2012).First();

Console.WriteLine("Aufgabe 1: ----------------------------------------------");
Console.WriteLine("Filme aus Jahr 2012 (First): " + query.Title);

// Aufgabe 2
var queryA2 = moviesCollection.AsQueryable()
    .Where(r => r.Actors.Contains("Pierce Brosnan")).ToList();

Console.WriteLine("Aufgabe 2: ----------------------------------------------");
Console.WriteLine("Filme mit Pierce Brosnan (Liste): ");
foreach (var item in queryA2)
{
    Console.WriteLine("- " + item.Title);
}



