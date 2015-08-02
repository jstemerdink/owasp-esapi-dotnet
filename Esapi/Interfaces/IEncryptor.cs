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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IEncryptor interface provides a set of methods for performing common
    ///     encryption, random number, and hashing operations. Implementors should take care
    ///     to ensure that they initialize their implementation with a strong "master key", and
    ///     that they protect this secret as much as possible.
    /// </summary>
    public interface IEncryptor
    {
        /// <summary>
        ///     Gets a timestamp representing the current date and time to be used by
        ///     other functions in the library.
        /// </summary>
        /// <returns>
        ///     The timestamp in long format.
        /// </returns>
        long TimeStamp { get; }

        /// <summary>
        ///     Returns a string representation of the hash of the provided plaintext and
        ///     salt. The salt helps to protect against a rainbow table attack by mixing
        ///     in some extra data with the plaintext.
        /// </summary>
        /// <param name="plaintext">
        ///     The plaintext.
        /// </param>
        /// <param name="salt">
        ///     The salt.
        /// </param>
        /// <returns>
        ///     The salted and hashed value.
        /// </returns>
        string Hash(string plaintext, string salt);

        /// <summary>
        ///     Encrypts the provided plaintext and returns a ciphertext string.
        /// </summary>
        /// <param name="plaintext">
        ///     The unencrypted value (plaintext).
        /// </param>
        /// <returns>
        ///     The encrypted value (ciphertext).
        /// </returns>
        string Encrypt(string plaintext);

        /// <summary>
        ///     Decrypts the provided ciphertext string (encrypted with the encrypt
        ///     method) and returns a plaintext string.
        /// </summary>
        /// <param name="ciphertext">
        ///     The encrypted value (ciphertext).
        /// </param>
        /// <returns>
        ///     The unencrypted value (plaintext).
        /// </returns>
        string Decrypt(string ciphertext);

        /// <summary>
        ///     Create a digital signature for the provided data and return it in a
        ///     string.
        /// </summary>
        /// <param name="data">
        ///     The data to sign.
        /// </param>
        /// <returns>
        ///     The signature.
        /// </returns>
        string Sign(string data);

        /// <summary>
        ///     Verifies a digital signature (created with the sign method) and returns
        ///     the bool result.
        /// </summary>
        /// <param name="signature">
        ///     The signature to verify.
        /// </param>
        /// <param name="data">
        ///     The data to verify the signature against.
        /// </param>
        /// <returns>
        ///     true, if successful
        /// </returns>
        bool VerifySignature(string signature, string data);

        /// <summary>
        ///     Creates a seal that binds a set of data and an expiration timestamp.
        /// </summary>
        /// <param name="data">
        ///     The data to seal.
        /// </param>
        /// <param name="timestamp">
        ///     The timestamp of the expiration date of the data.
        /// </param>
        /// <returns>
        ///     The seal value.
        /// </returns>
        string Seal(string data, long timestamp);

        /// <summary>
        ///     Unseals data (created with the seal method) and throws an exception
        ///     describing any of the various problems that could exist with a seal, such
        ///     as an invalid seal format, expired timestamp, or decryption error.
        /// </summary>
        /// <param name="seal">
        ///     The data to unseal.
        /// </param>
        /// <returns>
        ///     The data if the seal was valid.
        /// </returns>
        string Unseal(string seal);

        /// <summary>
        ///     Verifies a seal (created with the seal method) and throws an exception
        ///     describing any of the various problems that could exist with a seal, such
        ///     as an invalid seal format, expired timestamp, or data mismatch.
        /// </summary>
        /// <param name="seal">
        ///     The seal.
        /// </param>
        /// <returns>True, if the seal was valid. False, otherwise.</returns>
        bool VerifySeal(string seal);
    }
}
