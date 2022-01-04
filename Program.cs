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
redmond.CityName = "Redmond";
redmond.Location = new Point(-122.123889, 47.669444) { SRID = 4326 };
ctx.Cities.Add(redmond);
ctx.SaveChanges();

// Create factory 
var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

// Set our Current Location
var currentLocation = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-122.171936, 47.643159));

// Find the nearest city in correlation to our location
var nearestCity = ctx.Cities.OrderBy(c => c.Location.Distance(gf.CreateGeometry(currentLocation)));
// Print out
Console.WriteLine(String.Format("Nearest Citiy is : {0}", nearestCity.ToList().FirstOrDefault().CityName));

Console.ReadLine();
