﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.GlobalExceptions
{
    public class InvalidEmailException(string message) : Exception(message) { }
}
