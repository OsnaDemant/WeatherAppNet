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
            optionsBuilder.UseSqlite("Data Source=Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<Weather[], string>(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Weather[]>(v));

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WeatherDataQuery>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Clouds>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WeatherData>()
                .Property(e => e.Weather)
                .HasConversion(converter);

            modelBuilder.Entity<Sys>()
                .HasKey(x => x.DataBaseId);
            modelBuilder.Entity<WeatherData>().HasKey(x => x.IdWeatherData);
        }
    }
}
