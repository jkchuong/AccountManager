﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountData
{
    public partial class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int ThemeId { get; set; } = 1;
    }
}
