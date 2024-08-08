using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "Name must be in 5 letters only")]
        public string Name { get; set; }

        [Required]
        public int Salary { get; set; }
        public string Location { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Department { get; set; }
        public string Qualification { get; set; }

        [DisplayFormat(DataFormatString ="{0:MM:DD:YYYY}")]
        public DateTime Createddate { get; set; }
        public DateTime Updateddate { get; set; }
    }
}
