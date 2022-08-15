using System;
using System.ComponentModel.DataAnnotations;

namespace PaginationCacheAPI
{
    public class PersonDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }


        [MaxLength(500)]
        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "StaffId")]
        public string StaffId { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [EmailAddress]
        [MaxLength(500)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Phone]
        [MaxLength(25)]
        public string Phone { get; set; }

        [Required]
        [DateOfBirth]
        [DataType(DataType.Date)]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }


        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
