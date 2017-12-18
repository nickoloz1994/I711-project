﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseProject.Models
{
    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Budget Name")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(0.1, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        [NotMapped]
        public decimal RemainingBudget
        {
            get
            {
                var expenses = 0M;
                foreach (var expense in Expenses)
                {
                    expenses += expense.Amount;
                }
                return Amount - expenses;
            }
        }

        public decimal SumExpenses
        {
            get
            {
                var expenses = 0M;
                foreach (var expense in Expenses)
                {
                    expenses += expense.Amount;
                }
                return expenses;
            }
        }
    }
}