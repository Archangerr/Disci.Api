using DisciApp.Api.Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DisciApp.Api.DataBaseContext
{
    public class RezervasyonDbContext : DbContext
    {
        public DbSet<Rezervasyon> Rezervasyonlar { get; set; }

        public RezervasyonDbContext(DbContextOptions<RezervasyonDbContext> options) : base(options)
        {
        }

    }
}
