using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Interface;
using Presentation.Models;

namespace Presentation.Repository
{
    public class UserRepository:BaseRepository<ViewerModel>,IUser
    {
        private readonly PresentationDbContext myDbContext;

        public UserRepository(PresentationDbContext myDbContext) : base(myDbContext)
        {
            this.myDbContext = myDbContext;
        }
    }
}
