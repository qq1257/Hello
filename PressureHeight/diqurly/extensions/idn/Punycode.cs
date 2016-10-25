// Copyright ?2006 - 2012 Dieter Lunn
// Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
// You should have received a copy of the GNU Lesser General Public License along
// with this library; if not, write to the Free Software Foundation, Inc., 59
// Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Text;

namespace Gnu.Inet.Encoding
{
    /// <summary>
    /// The punycode.
    /// </summary>
    public class Punycode
    {
        /* Punycode parameters */

        /// <summary>
        /// The tmin.
        /// </summary>
        internal const int TMIN = 1;

        /// <summary>
        /// The tmax.
        /// </summary>
        internal const int TMAX = 26;

        /// <summary>
        /// The base.
        /// </summary>
        internal const int BASE = 36;

        /// <summary>
        /// The initia l_ n.
        /// </summary>
        internal const int INITIAL_N = 128;

        /// <summary>
        /// The initia l_ bias.
        /// </summary>
        internal const int INITIAL_BIAS = 72;

        /// <summary>
        /// The damp.
        /// </summary>
        internal const int DAMP = 700;

        /// <summary>
        /// The skew.
        /// </summary>
        internal const int SKEW = 38;

        /// <summary>
        /// The delimiter.
        /// </summary>
        internal const char DELIMITER = '-';

        /// <summary>
        /// Punycodes a unicode string.
        /// </summary>
        /// <param name="input">
        /// Unicode string.
        /// </param>
        /// <returns>
        /// Punycoded string.
        /// </returns>
        public static string Encode(string input)
        {
            int n = INITIAL_N;
            int delta = 0;
            int bias = INITIAL_BIAS;
            var output = new StringBuilder();

            // Copy all basic code points to the output
            int b = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (IsBasic(c))
                {
                    output.Append(c);
                    b++;
                }
            }

            // Append delimiter
            if (b > 0)
            {
                output.Append(DELIMITER);
            }

            int h = b;
            while (h < input.Length)
            {
                int m = int.MaxValue;

                // Find the minimum code point >= n
                for (int i = 0; i < input.Length; i++)
                {
                    int c = input[i];
                    if (c >= n && c < m)
                    {
                        m = c;
                    }
                }

                if (m - n > (int.MaxValue - delta)/(h + 1))
                {
                    throw new PunycodeException(PunycodeException.OVERFLOW);
                }

                delta = delta + (m - n)*(h + 1);
                n = m;

                for (int j = 0; j < input.Length; j++)
                {
                    int c = input[j];
                    if (c < n)
                    {
                        delta++;
                        if (0 == delta)
                        {
                            throw new PunycodeException(PunycodeException.OVERFLOW);
                        }
                    }

                    if (c == n)
                    {
                        int q = delta;

                        for (int k = BASE;; k += BASE)
                        {
                            int t;
                            if (k <= bias)
                            {
                                t = TMIN;
                            }
                            else if (k >= bias + TMAX)
                            {
                                t = TMAX;
                            }
                            else
                            {
                                t = k - bias;
                            }

                            if (q < t)
                            {
                                break;
                            }

                            output.Append((char) Digit2Codepoint(t + (q - t)%(BASE - t)));
                            q = (q - t)/(BASE - t);
                        }

                        output.Append((char) Digit2Codepoint(q));
                        bias = Adapt(delta, h + 1, h == b);
                        delta = 0;
                        h++;
                    }
                }

                delta++;
                n++;
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode a punycoded string.
        /// </summary>
        /// <param name="input">
        /// Punycode string
        /// </param>
        /// <returns>
        /// Unicode string.
        /// </returns>
        public static string Decode(string input)
        {
            int n = INITIAL_N;
            int i = 0;
            int bias = INITIAL_BIAS;
            var output = new StringBuilder();

            int d = input.LastIndexOf(DELIMITER);
            if (d > 0)
            {
                for (int j = 0; j < d; j++)
                {
                    char c = input[j];
                    if (!IsBasic(c))
                    {
                        throw new PunycodeException(PunycodeException.BAD_INPUT);
                    }

                    output.Append(c);
                }

                d++;
            }
            else
            {
                d = 0;
            }

            while (d < input.Length)
            {
                int oldi = i;
                int w = 1;

                for (int k = BASE;; k += BASE)
                {
                    if (d == input.Length)
                    {
                        throw new PunycodeException(PunycodeException.BAD_INPUT);
                    }

                    int c = input[d++];
                    int digit = Codepoint2Digit(c);
                    if (digit > (int.MaxValue - i)/w)
                    {
                        throw new PunycodeException(PunycodeException.OVERFLOW);
                    }

                    i = i + digit*w;

                    int t;
                    if (k <= bias)
                    {
                        t = TMIN;
                    }
                    else if (k >= bias + TMAX)
                    {
                        t = TMAX;
                    }
                    else
                    {
                        t = k - bias;
                    }

                    if (digit < t)
                    {
                        break;
                    }

                    w = w*(BASE - t);
                }

                bias = Adapt(i - oldi, output.Length + 1, oldi == 0);

                if (i/(output.Length + 1) > int.MaxValue - n)
                {
                    throw new PunycodeException(PunycodeException.OVERFLOW);
                }

                n = n + i/(output.Length + 1);
                i = i%(output.Length + 1);

// following overload is not supported on CF
                // output.Insert(i,(char) n);
                output.Insert(i, new[] {(char) n});
                i++;
            }

            return output.ToString();
        }

        /// <summary>
        /// The adapt.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        /// <param name="numpoints">
        /// The numpoints.
        /// </param>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Adapt(int delta, int numpoints, bool first)
        {
            if (first)
            {
                delta = delta/DAMP;
            }
            else
            {
                delta = delta/2;
            }

            delta = delta + (delta/numpoints);

            int k = 0;
            while (delta > ((BASE - TMIN)*TMAX)/2)
            {
                delta = delta/(BASE - TMIN);
                k = k + BASE;
            }

            return k + ((BASE - TMIN + 1)*delta)/(delta + SKEW);
        }

        /// <summary>
        /// The is basic.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsBasic(char c)
        {
            return c < 0x80;
        }

        /// <summary>
        /// The digit 2 codepoint.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="PunycodeException">
        /// </exception>
        public static int Digit2Codepoint(int d)
        {
            if (d < 26)
            {
                // 0..25 : 'a'..'z'
                return d + 'a';
            }

            if (d < 36)
            {
                // 26..35 : '0'..'9';
                return d - 26 + '0';
            }

            throw new PunycodeException(PunycodeException.BAD_INPUT);
        }

        /// <summary>
        /// The codepoint 2 digit.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="PunycodeException">
        /// </exception>
        public static int Codepoint2Digit(int c)
        {
            if (c - '0' < 10)
            {
                // '0'..'9' : 26..35
                return c - '0' + 26;
            }

            if (c - 'a' < 26)
            {
                // 'a'..'z' : 0..25
                return c - 'a';
            }

            throw new PunycodeException(PunycodeException.BAD_INPUT);
        }
    }
}