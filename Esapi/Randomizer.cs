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
using System.Security.Cryptography;
using System.Text;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer" />
    /// <summary>
    ///     Reference implemenation of the <see cref="Owasp.Esapi.Interfaces.IRandomizer" /> interface. This implementation
    ///     builds on the MSCAPI provider to provide a
    ///     cryptographically strong source of entropy. The specific algorithm used is configurable in the ESAPI properties.
    /// </summary>
    public class Randomizer : IRandomizer, IDisposable
    {
        private readonly RandomNumberGenerator randomNumberGenerator;

        /// <summary>
        ///     Instantiates the class, with the apropriate algorithm.
        /// </summary>
        public Randomizer()
        {
            string algorithm = Esapi.SecurityConfiguration.RandomAlgorithm;
            try
            {
                this.randomNumberGenerator = RandomNumberGenerator.Create(algorithm);
            }
            catch (Exception e)
            {
                throw new EncryptionException(
                    EM.Randomizer_Failure,
                    string.Format(EM.Randomizer_AlgCreateFailed1, algorithm),
                    e);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomBoolean()" />
        public bool GetRandomBoolean()
        {
            byte[] randomByte = new byte[1];
            this.randomNumberGenerator.GetBytes(randomByte);
            return (randomByte[0] >= 128);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomGUID()" />
        public Guid GetRandomGUID()
        {
            string guidString = string.Format(
                "{0}-{1}-{2}-{3}-{4}",
                this.GetRandomString(8, CharSetValues.Hex),
                this.GetRandomString(4, CharSetValues.Hex),
                this.GetRandomString(4, CharSetValues.Hex),
                this.GetRandomString(4, CharSetValues.Hex),
                this.GetRandomString(12, CharSetValues.Hex));
            return new Guid(guidString);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomString(int, char[])" />
        public string GetRandomString(int length, char[] characterSet)
        {
            StringBuilder sb = new StringBuilder();

            for (int loop = 0; loop < length; loop++)
            {
                int index = this.GetRandomInteger(0, characterSet.GetLength(0) - 1);
                sb.Append(characterSet[index]);
            }
            string nonce = sb.ToString();
            return nonce;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomInteger(int, int)" />
        public int GetRandomInteger(int min, int max)
        {
            double range = (double)max - min;
            byte[] randomBytes = new byte[sizeof(int)];
            this.randomNumberGenerator.GetBytes(randomBytes);
            uint randomFactor = BitConverter.ToUInt32(randomBytes, 0);
            double divisor = (double)randomFactor / uint.MaxValue;
            int randomNumber = Convert.ToInt32(Math.Round(range * divisor) + min);
            return randomNumber;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomDouble(double, double)" />
        public double GetRandomDouble(double min, double max)
        {
            // This method only gives you 32 bits of entropy (based of random int).
            double factor = max - min;
            double random = this.GetRandomInteger(0, int.MaxValue) / (double)int.MaxValue;
            return random * factor + min;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomFilename(string)" />
        public string GetRandomFilename(string extension)
        {
            return this.GetRandomString(12, CharSetValues.Alphanumerics) + "." + extension;
        }

        private static bool Contains(StringBuilder sb, char c)
        {
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == c)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.randomNumberGenerator.Dispose();
            }
        }

        // Disposable types implement a finalizer.
        ~Randomizer()
        {
            this.Dispose(false);
        }
    }
}
