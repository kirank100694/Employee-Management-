using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Column(TypeName = "nVarchar(30)")]
        public string Name { get; set; }

        public long Salary { get; set; }

        [Column(TypeName = "nVarchar(200)")]
        public string Location { get; set; }

        [Column(TypeName = "nVarchar(30)")]
        public string Email { get; set; }

        [Column(TypeName = "nVarchar(30)")]
        public string Department { get; set; }

        [Column(TypeName = "nVarchar(30)")]
        public string Qualification { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
