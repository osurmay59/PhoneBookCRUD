using Microsoft.EntityFrameworkCore;
using PhoneBookCRUD.Models;

namespace PhoneBookCRUD.Data
{
    public class ClientsDb : DbContext
    {
        public ClientsDb(DbContextOptions<ClientsDb> options) : base(options) { 
                
        }
        public DbSet<Client> Clients => Set<Client>();

    }
}
