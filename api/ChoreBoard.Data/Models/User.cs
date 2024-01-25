using System;
using System.Collections.Generic;

namespace ChoreBoard.Data.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
