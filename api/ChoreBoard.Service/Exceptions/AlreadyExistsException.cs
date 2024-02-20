﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreBoard.Service.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string message): base(message)
        {
        }
    }
}
