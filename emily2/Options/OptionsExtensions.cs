using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace emily2.Options
{
    internal static class OptionsExtensions
    {
        internal static UserOptions LoadUserRSA(this UserOptions userOptions)
        {
            if (userOptions == null) return null;

            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = userOptions.Email
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            userOptions.RSA = new RSACryptoServiceProvider(parameters);
            return userOptions;
        }
    }
}
