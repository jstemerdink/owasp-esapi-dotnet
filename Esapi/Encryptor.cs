// Copyright© 2015 OWASP.org. 
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.IEncryptor" /> IEncryptor interface. This
    ///     implementation
    ///     layers on the .NET provided cryptographic package.
    ///     Algorithms used are configurable in the configuration file.
    /// </summary>
    public class Encryptor : IEncryptor
    {
        /// <summary>The asymmetric key pair </summary>
        private readonly CspParameters asymmetricKeyPair;

        /// <summary>
        ///     The symmetric key
        /// </summary>
        private readonly byte[] secretKey;

        internal string encoding = Esapi.SecurityConfiguration.CharacterEncoding;

        internal string encryptAlgorithm = Esapi.SecurityConfiguration.EncryptionAlgorithm;

        internal string hashAlgorithm = Esapi.SecurityConfiguration.HashAlgorithm;

        internal string signatureAlgorithm = Esapi.SecurityConfiguration.DigitalSignatureAlgorithm;

        /// <summary>
        ///     Public constructor.
        /// </summary>
        public Encryptor()
        {
            string pass = Esapi.SecurityConfiguration.MasterPassword;
            byte[] salt = Esapi.SecurityConfiguration.MasterSalt;

            try
            {
                // Set up encryption and decryption                
                using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(this.encryptAlgorithm))
                {
                    using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(pass, salt))
                    {
                        this.secretKey = rfc2898.GetBytes(symmetricAlgorithm.KeySize / 8);
                    }
                    // TODO: Hardcoded value 13 is the code for DSA
                    this.asymmetricKeyPair = new CspParameters(13);

                    // The asymmetric key will be stored in the key container using the name ESAPI.
                    this.asymmetricKeyPair.KeyContainerName = "ESAPI";
                }
            }
            catch (Exception e)
            {
                throw new EncryptionException(EM.Encryptor_EncryptionFailure, EM.Encryptor_EncryptorCreateFailed, e);
            }
        }

        /// <summary>
        ///     Gets a timestamp representing the current date and time to be used by
        ///     other functions in the library.
        /// </summary>
        /// <returns>
        ///     The timestamp in long format.
        /// </returns>
        public long TimeStamp
        {
            get
            {
                return DateTime.Now.Ticks;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.Hash(string, string)" />
        public string Hash(string plaintext, string salt)
        {
            try
            {
                // Create a new instance of the hash crypto service provider.
                using (HashAlgorithm hasher = HashAlgorithm.Create(this.hashAlgorithm))
                {
                    // Convert the data to hash to an array of Bytes.
                    byte[] originalBytes = Encoding.UTF8.GetBytes(plaintext + salt);

                    // Compute the Hash. This returns an array of Bytes.
                    byte[] hashBytes = hasher.ComputeHash(originalBytes);

                    // rehash a number of times to help strengthen weak passwords
                    // FIXME: ENHANCE make iterations configurable                
                    for (int i = 0; i < 1024; i++)
                    {
                        hashBytes = hasher.ComputeHash(hashBytes);
                    }
                    // Optionally, represent the hash value as a base64-encoded string, 
                    // For example, if you need to display the value or transmit it over a network.
                    string hashBase64String = Convert.ToBase64String(hashBytes);

                    return hashBase64String;
                }
            }
            catch (Exception e)
            {
                throw new EncryptionException(
                    EM.Encryptor_EncryptionFailure,
                    string.Format(EM.Encryptor_WrongHashAlg1, this.hashAlgorithm),
                    e);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.Encrypt(string)" />
        public string Encrypt(string plaintext)
        {
            try
            {
                // Create a new key and initialization vector.
                // If a key is not provided, a key of appropriate length is
                // automatically generated. You can retrieve its value through the Key
                // property. Similarly, an initialization vector is automatically
                // generated if you do not specify one.

                // Get the encryptor.
                using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(this.encryptAlgorithm))
                {
                    symmetricAlgorithm.GenerateIV();
                    byte[] iv = symmetricAlgorithm.IV;

                    ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(this.secretKey, iv);
                    // Define a new CryptoStream object to hold the encrypted bytes and encrypt the data.

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                        try
                        {
                            // Convert the data to a byte array.
                            Encoding textConverter = Encoding.GetEncoding(this.encoding);
                            byte[] plaintextBytes = textConverter.GetBytes(plaintext);
                            // Encrypt the data by writing it to the CryptoStream object.
                            // Write all data to the crypto stream and flush it.
                            csEncrypt.Write(plaintextBytes, 0, plaintextBytes.Length);
                            csEncrypt.FlushFinalBlock();

                            // Get encrypted array of bytes from the memory stream.
                            byte[] encryptedBytes = msEncrypt.ToArray();
                            byte[] encryptedBytesPlusIv = Combine(iv, encryptedBytes);
                            return Convert.ToBase64String(encryptedBytesPlusIv);
                        }
                        finally
                        {
                            symmetricAlgorithm.Clear();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new EncryptionException(
                    EM.Encryptor_EncryptionFailure,
                    string.Format(EM.Encryptor_DecryptFailed1, e.Message),
                    e);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.Decrypt(string)" />
        public string Decrypt(string ciphertext)
        {
            // Note - Cipher is not threadsafe so we create one locally
            try
            {
                using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(this.encryptAlgorithm))
                {
                    byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

                    // Ciphertext actually includes the IV, so we need to split it out
                    // Get first part of array, which is IV
                    int ivLength = symmetricAlgorithm.IV.Length;
                    byte[] ivBytes = new byte[ivLength];
                    Array.Copy(ciphertextBytes, ivBytes, ivLength);

                    // Get second part of array which is actual ciphertext
                    int onlyCiphertextLength = ciphertextBytes.Length - ivLength;
                    byte[] onlyCiphertextBytes = new byte[onlyCiphertextLength];
                    Array.Copy(ciphertextBytes, ivLength, onlyCiphertextBytes, 0, onlyCiphertextLength);

                    ICryptoTransform decryptor = symmetricAlgorithm.CreateDecryptor(this.secretKey, ivBytes);

                    // Now decrypt the previously encrypted data using the decryptor.
                    using (MemoryStream msDecrypt = new MemoryStream(onlyCiphertextBytes))
                    {
                        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                        try
                        {
                            // Read the data out of the crypto stream.
                            byte[] plaintextBytes = new byte[onlyCiphertextLength];
                            int decryptedBytes = csDecrypt.Read(plaintextBytes, 0, onlyCiphertextLength);

                            // Convert the byte array back into a string.
                            Encoding textConverter = Encoding.GetEncoding(this.encoding);
                            string plaintext = textConverter.GetString(plaintextBytes, 0, decryptedBytes);
                            return plaintext;
                        }
                        finally
                        {
                            symmetricAlgorithm.Clear();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new EncryptionException(
                    EM.Encryptor_DecryptionFailure,
                    string.Format(EM.Encryptor_DecryptFailed1, e.Message),
                    e);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.Sign(string)" />
        public string Sign(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            // Since this is the only asymmetric algorithm with signing capabilities, its hardcoded.
            // The more general APIs just aren't friendly.
            using (DSACryptoServiceProvider dsaCsp = new DSACryptoServiceProvider(this.asymmetricKeyPair))
            {
                Encoding textConverter = Encoding.GetEncoding(this.encoding);
                byte[] dataBytes = textConverter.GetBytes(data);
                byte[] signatureBytes = dsaCsp.SignData(dataBytes);
                return Convert.ToBase64String(signatureBytes);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.VerifySignature(string, string)" />
        public bool VerifySignature(string signature, string data)
        {
            try
            {
                using (DSACryptoServiceProvider dsaCsp = new DSACryptoServiceProvider(this.asymmetricKeyPair))
                {
                    Encoding textConverter = Encoding.GetEncoding(this.encoding);
                    byte[] signatureBytes = Convert.FromBase64String(signature);
                    byte[] dataBytes = textConverter.GetBytes(data);
                    return dsaCsp.VerifyData(dataBytes, signatureBytes);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Creates a seal that binds a set of data and an expiration timestamp.
        /// </summary>
        /// <param name="data">
        ///     The data to seal.
        /// </param>
        /// <param name="expiration">
        ///     The timestamp of the expiration date of the data.
        /// </param>
        /// <returns>
        ///     The seal value.
        /// </returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IEncryptor.Seal(string, long)">
        /// </seealso>
        public string Seal(string data, long expiration)
        {
            try
            {
                return this.Encrypt(expiration + ":" + data);
            }
            catch (EncryptionException e)
            {
                throw new IntegrityException(e.UserMessage, e.LogMessage, e);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.Unseal(string)" />
        public string Unseal(string seal)
        {
            string plaintext = null;
            try
            {
                plaintext = this.Decrypt(seal);
            }
            catch (EncryptionException e)
            {
                throw new EncryptionException(EM.Encryptor_InvalidSeal, EM.Encryptor_SealDecryptFailed, e);
            }

            int index = plaintext.IndexOf(":");
            if (index == -1)
            {
                throw new EncryptionException(EM.Encryptor_InvalidSeal, EM.Encryptor_SealWrongFormat);
            }

            string timestring = plaintext.Substring(0, index);
            long now = DateTime.Now.Ticks;
            long expiration = Convert.ToInt64(timestring);
            if (now > expiration)
            {
                throw new EncryptionException(EM.Encryptor_InvalidSeal, EM.Encryptor_SealExpired);
            }

            index = plaintext.IndexOf(":", index + 1);
            string sealedValue = plaintext.Substring(index + 1);
            return sealedValue;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncryptor.VerifySeal(string)" />
        public bool VerifySeal(string seal)
        {
            try
            {
                this.Unseal(seal);
                return true;
            }
            catch (EncryptionException)
            {
                return false;
            }
        }

        private static byte[] Combine(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }
}
