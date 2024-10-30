﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class HashHelper
    {
        public static string ComputeHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyHash(string enteredPassword, string storedHash)
        {
            var enteredHash = ComputeHash(enteredPassword);
            return enteredHash == storedHash;
        }
    }
}
