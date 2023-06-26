using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Milestone.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Select Sex:")]
        public String Sex { get; set; }

        [Required]
        [Range(1, 99)]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

        public string LoginToString()
        {
            return "\nUsername: " + UserName + "\nPassword: " + Password;
        }

        public string RegistrationToString()
        {
            return "\nId: " + Id + "\nFirst Name: " + FirstName + "\nLast Name: " +
                LastName + "\nUsername: " + UserName + "\nPassword: " + Password +
                "\nSex: " + Sex + "\nAge: " + Age + "\nState: " + State + "\nEmail: " + Email;
        }

    }
}