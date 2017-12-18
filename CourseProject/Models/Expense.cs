using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }

        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [StringLength(200)]
        public string Note { get; set; }

        public int BudgetId { get; set; }
        public Budget Budget { get; set; }
    }
}