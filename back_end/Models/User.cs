using System;
using System.Collections.Generic;

namespace back_end.Models {
    public class Users {
        public int UserID { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string lastNames { get; set; }
        public string BirthDate { get; set; }
        public string UserPassword { get; set; }
    }
}