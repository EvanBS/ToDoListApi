﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class User
    {
        
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        
        public virtual List<TodoItem> TodoItems { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }


    }
}
