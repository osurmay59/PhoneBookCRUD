using Microsoft.EntityFrameworkCore;

namespace PhoneBookCRUD.Models
{
    [PrimaryKey(nameof(Id))]
    public class Client
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public int NotShow { get; set; }




    }
}
