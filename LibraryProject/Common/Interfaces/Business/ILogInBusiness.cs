﻿using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces.Business
{
    public interface ILogInBusiness
    {
        Role LogIn(User user);
        string GetNameUser(string jmbg);
    }
}
