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

// Aufgabe 3
Movie newMovie = new Movie();
newMovie.Title = "The Da Vinci Code";
newMovie.Actors = new List<string>(){"Tom Hanks","Audrey Tautou"};
newMovie.Summary = "So dunkel ist der Betrug an der Menschheit";
newMovie.Year = 2006;

moviesCollection.InsertOne(newMovie);

// Aufgabe 4

var newMovies = new List<Movie>();

var myMovie1 = new Movie();
myMovie1.Title = "Ocean's Eleven";
myMovie1.Actors = new List<string>(){"George Clooney", "Brad Pitt", "Julia Roberts"};
myMovie1.Summary = "Bist du drin oder draussen?";
myMovie1.Year = 2001;

newMovies.Add(myMovie1);

var myMovie2 = new Movie();
myMovie2.Title = "Ocean's Twelve";
myMovie2.Actors = new List<string>(){"George Clooney", "Brad Pitt", "Julia Roberts", "Andy Garcia"};
myMovie2.Summary = "Die Elf sind jetzt Zwölf.";
myMovie2.Year = 2004;

newMovies.Add(myMovie2);

moviesCollection.InsertMany(newMovies);


// Aufgabe 5
var updateFilter = Builders<Movie>.Filter.Eq(f => f.Title, "Skyfall - 007");
var update = Builders<Movie>.Update
    .Set(d => d.Title, "Skyfall");

var result = moviesCollection.UpdateMany(updateFilter, update);


// Aufgabe 6
var deleteFilter = Builders<Movie>.Filter.Lte(f => f.Year, 1995);
var deleteResult = moviesCollection.DeleteMany(deleteFilter);
Console.WriteLine("Aufgabe H: ----------------------------------------------");
Console.WriteLine("Delete Year <= 1995 (Anzahl): " + deleteResult.DeletedCount); 
Console.WriteLine("");


// Aufgabe 7
var aggregateResult = moviesCollection.Aggregate()
    .Match(m => m.Year >= 2000)
    .Group( m => m.Year, g => new{ Jahr = g.Key, Anzahl=g.Count()})
    .SortBy(m => m.Jahr)
    .ToList();

Console.WriteLine("Aufgabe I (Zusatzaufgabe): ------------------------------");
Console.WriteLine("Filme pro Jahr ab 2000");
foreach (var item in aggregateResult)
{
    Console.WriteLine("- " + item.Jahr + " " + item.Anzahl);
}
Console.WriteLine("");