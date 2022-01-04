using Microsoft.EntityFrameworkCore;
using spatial;

Console.WriteLine("Spatial, Demo!");
DbContextOptionsBuilder opt = new DbContextOptionsBuilder();
opt.UseSqlServer(@"Server=localhost,60666;Initial Catalog=Spatial;Persist Security Info=False;User ID=sa;Password=Schneemann1;MultipleActiveResultSets=False;Connection Timeout=30;", 
    // Using topolgy
    x => x.UseNetTopologySuite());

DataBaseContext ctx = new DataBaseContext(opt.Options);

// Create database
ctx.Database.EnsureCreated();



