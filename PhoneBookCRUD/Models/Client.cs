using Microsoft.EntityFrameworkCore;

namespace PhoneBookCRUD.Models
{
    [PrimaryKey(nameof(PhoneNumber))]
    public class Client
    {
        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public int NotShow { get; set; }




    }
}
