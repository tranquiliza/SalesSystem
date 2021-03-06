﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public interface ISecurity
    {
        bool TryCreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
    }
}
