﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FINANCE.Shared.Auth
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string refreshToken { get; set; }
        public bool Result { get; set; }

        public List<string> Errors { get; set; }
    }
}
