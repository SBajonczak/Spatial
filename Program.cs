using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;
using System.Linq;
using spatial;

Console.WriteLine("Spatial, Demo!");
DbContextOptionsBuilder opt = new DbContextOptionsBuilder();
opt.UseSqlServer(@"Server=localhost,60666;Initial Catalog=Spatial;Persist Security Info=False;User ID=sa;Password=Schneemann1;MultipleActiveResultSets=False;Connection Timeout=30;", 
    // Using topolgy
    x => x.UseNetTopologySuite());

DataBaseContext ctx = new DataBaseContext(opt.Options);

// Create database
ctx.Database.EnsureCreated();
// Celanup the db for multiple runs
if (ctx.Cities.Any())
{
    ctx.Cities.RemoveRange(ctx.Cities);
    ctx.SaveChanges();
}

// Create first data entry
City seattle = new City();
seattle.CityName = "Seattle";
seattle.Location = new Point(-122.333056, 47.609722) { SRID = 4326 };
ctx.Cities.Add(seattle);
ctx.SaveChanges();

// Create second data entry
City redmond = new City();
redmond.CityName = "Redmond (Center)";
redmond.Location = new Point(-122.125569, 47.674222) { SRID = 4326 };
ctx.Cities.Add(redmond);
ctx.SaveChanges();


City poi1 = new City();
poi1.CityName = "Poi 1 (15990 NE 85th St)";
poi1.Location = new Point(-122.128070, 47.678498) { SRID = 4326 };
ctx.Cities.Add(poi1);
ctx.SaveChanges();

City poi2 = new City();
poi2.CityName = "Poi 2 (EvergreenHealth Medical Center Kirkland at Kirkland)";
poi2.Location = new Point(-122.178200, 47.715305) { SRID = 4326 };
ctx.Cities.Add(poi2);
ctx.SaveChanges();


// Create factory with SRID 4326
var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

// Current Locatin is "Remond Way" in Redmond
// Set our Current Location
var currentLocation = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-122.127812, 47.675470));


// Find the nearest city in correlation to our location
var nearestCity = ctx.Cities.OrderBy(c => c.Location.Distance(currentLocation));
// Print out
Console.WriteLine(String.Format("Nearest City is : {0}", nearestCity.ToList().FirstOrDefault().CityName));
Console.WriteLine(String.Format("======================"));
Console.WriteLine(String.Format("Distance to targets (in miles)"));


// Find the nearest city in correlation to our location
var cities = ctx.Cities;

foreach (var city in cities)
{
    var distanceInKM = city.Location.Distance(currentLocation) * 1000;// meters ;
    Console.WriteLine(String.Format("Destination to : {0} is {1} ", city.CityName, distanceInKM));

}

Console.WriteLine(String.Format("======================"));
Console.WriteLine(String.Format("Objects in 2 KM radius"));

var distanceInMeters = 2 * 1609.344;

var near2Kilometers = ctx.Cities.Where(_=> _.Location.IsWithinDistance(currentLocation, distanceInMeters));

foreach (var city in near2Kilometers)
{
    var distanceInKM = city.Location.Distance(currentLocation) * 1000;
    Console.WriteLine(String.Format("Found Poi's: {0} is {1}  {2}", city.CityName, distanceInKM, city.Location.AsText()));

}


Console.ReadLine();
