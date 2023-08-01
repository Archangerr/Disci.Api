using DisciApp.Api.Entity;
using Microsoft.EntityFrameworkCore;

namespace DisciApp.Api.DataBaseContext
{
    public class ImgDbContext : DbContext
    {
        public DbSet<Img> Imgs { get; set; }

        public ImgDbContext(DbContextOptions<ImgDbContext> options) : base(options)
        {
        }
    }
    
}
