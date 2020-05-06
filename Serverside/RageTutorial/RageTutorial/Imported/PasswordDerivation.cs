using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RageTutorial.Imported {
    class PasswordDerivation {

        // Here we define some constants that are used within the generation of the password.
        // You may increase defaultIterations to make the computation slower(and thus harder to brute-force), which would be needed in the future when hardware gets better..
        public const int defaultSaltSize = 16;
        public const int defaultKeySize = 16;
        public const int defaultIterations = 10000;

        // This function takes our plain text password, and hashes it, salting it on the way, 
        // returning the salted hashed password, the salt itself, and some other computation parameters, encoded in a single string.
        public static string Derive(string plainPassword, int saltSize = defaultSaltSize, int iterations = defaultIterations, int keySize = defaultKeySize) {
            // The key derivation class automatically generates the salt using the given parameter
            using (var derive = new Rfc2898DeriveBytes(plainPassword, saltSize: saltSize, iterations: iterations)) {
                // These functions generate raw byte arrays, we encode them as base-64 strings so that they can easily be stored in most databases.
                var b64Pwd = Convert.ToBase64String(derive.GetBytes(keySize));
                var b64Salt = Convert.ToBase64String(derive.Salt);
                // note that we also include the iteration count and key size, so when verifying the password, we'd use them, instead of the class constant values which we may change in the future.
                return string.Join(":", b64Salt, iterations.ToString(), keySize.ToString(), b64Pwd);
            }
        }
        public static bool Verify(string saltedPassword, string plainPassword) {

            var passwordParts = saltedPassword.Split(':');
            // we convert our strings back into raw bytes/integers.
            var salt = Convert.FromBase64String(passwordParts[0]);
            var iters = int.Parse(passwordParts[1]);
            var keySize = int.Parse(passwordParts[2]);
            var pwd = Convert.FromBase64String(passwordParts[3]);
            // we generate a salted hashed password with the user input 'plainPassword', using the salt and the computation constants of the original password.
            using (var derive = new Rfc2898DeriveBytes(plainPassword, salt: salt, iterations: iters)) {
                var hashedInput = derive.GetBytes(keySize);
                // we ensure that the resulting salted hash is equal to our original hash, if so, the two passwords match.
                return hashedInput.SequenceEqual(pwd);
            }
        }
    }
}
