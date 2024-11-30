using System;
using Presentation.Data;
using Presentation.Interface;
using Presentation.Models;
using Presentation.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Presentation.Interface;
namespace Presentation.Repository
{
    public class PresentationRepository : BaseRepository<PresentationModel>, IPresentation
    {
    
        private readonly PresentationDbContext myDbContext;

        public PresentationRepository(PresentationDbContext myDbContext) : base(myDbContext)
        {
            this.myDbContext = myDbContext;
        }
        
    }
}
