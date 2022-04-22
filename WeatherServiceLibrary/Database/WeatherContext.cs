using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;
using WeatherServiceLibrary.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace WeatherServiceLibrary.Database
{
    
    public class WeatherContext : DbContext
    {
        
        public DbSet<WeatherDataQuery> WeatherDataQuerys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("DataBase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<Weather[], string>(
                v => string.Join(";", JsonConvert.SerializeObject(v)),
                v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(val => JsonConvert.DeserializeObject<Weather>(val)).ToArray());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WeatherDataQuery>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Clouds>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WeatherData>()
                .Property(e => e.Weather)
                .HasConversion(converter);
        }
    }
}
