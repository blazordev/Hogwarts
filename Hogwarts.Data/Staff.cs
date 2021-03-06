﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hogwarts.Data
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        public string MiddleNames { get; set; } = "";
        [Required]
        [MaxLength(50)] 
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string ImageLink { get; set; }
        public IEnumerable<StaffRole> StaffRoles { get; set; } = new List<StaffRole>();
        public IEnumerable<StaffCourse> StaffCourse { get; set; } = new List<StaffCourse>();


    }
}
