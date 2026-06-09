using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }

        [Range(0.01, 999999999.99)]
        [Column(TypeName = "NUMBER(12,2)")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }
    }
}
