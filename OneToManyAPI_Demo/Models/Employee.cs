using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OneToManyAPI_Demo.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public int EmployeeName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
