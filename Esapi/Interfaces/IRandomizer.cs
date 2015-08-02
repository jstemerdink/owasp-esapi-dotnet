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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IRandomizer interface defines a set of methods for creating
    ///     cryptographically random numbers and strings.
    /// </summary>
    public interface IRandomizer
    {
        /// <summary> Returns a random bool.</summary>
        bool GetRandomBoolean();

        /// <summary> Generates a random GUID.</summary>
        Guid GetRandomGUID();

        /// <summary>
        ///     Gets a random string.
        /// </summary>
        /// <param name="length">
        ///     The desired length.
        /// </param>
        /// <param name="characterSet">
        ///     The desired character set.
        /// </param>
        /// <returns>
        ///     The random string.
        /// </returns>
        string GetRandomString(int length, char[] characterSet);

        /// <summary>
        ///     Gets a random integer.
        /// </summary>
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        /// <returns>
        ///     The random integer
        /// </returns>
        int GetRandomInteger(int min, int max);

        /// <summary>
        ///     Returns an unguessable filename.
        /// </summary>
        /// <param name="extension">The extension for the filename</param>
        /// <returns>The unguessable filename</returns>
        string GetRandomFilename(string extension);

        /// <summary>
        ///     Gets a random double.
        /// </summary>
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        /// <returns>
        ///     The random double.
        /// </returns>
        double GetRandomDouble(double min, double max);
    }
}
