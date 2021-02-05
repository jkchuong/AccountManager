using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ManagementCode
{
    public partial class User
    {
        public string Name { get; set; }

        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int ThemeId { get; set; }
    }
}
