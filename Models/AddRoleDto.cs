﻿namespace Movie.Models
{
    public class AddRoleDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
