﻿using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IAuthorizationRL
    {
        public string GenerateJwtToken(User user);
    }
}
