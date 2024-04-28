using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("dblog")]
    public class LoginModel
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
