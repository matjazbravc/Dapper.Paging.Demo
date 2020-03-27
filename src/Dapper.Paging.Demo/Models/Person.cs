using System;
using System.ComponentModel.DataAnnotations;

namespace Dapper.Paging.Demo.Models
{
    /// <summary>
    /// Simplified Person model/entity
    /// </summary>
    [Serializable]
    public class Person
    {
        [Key]
        [Display(Name = "Person Id")]
        public int BusinessEntityID { get; set; }

        [Required]
        [Display(Name = "Person Type")]
        public string PersonType { get; set; }

        [Display(Name = "Person Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Person First name")]
        public string FirstName { get; set; }

        [Display(Name = "Person First name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Person LAst name")]
        public string LastName { get; set; }

        [Display(Name = "Person Sufix")]
        public string Suffix { get; set; }

        [Display(Name = "Modified")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
