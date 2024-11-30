using Microsoft.EntityFrameworkCore;
using Presentation.Models;

namespace Presentation.Data
{
    public class PresentationDbContext:DbContext
    {
        public PresentationDbContext(DbContextOptions<PresentationDbContext> options):base(options)
        {
            
        }
        DbSet<PresentationModel> presentationsModel { get; set; }   
        DbSet<ViewerModel> ViewerModel { get; set; }

    }
}
