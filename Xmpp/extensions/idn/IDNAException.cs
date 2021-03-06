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

namespace Gnu.Inet.Encoding
{
    /// <summary>
    /// The idna exception.
    /// </summary>
    public class IDNAException : Exception
    {
        /// <summary>
        /// The contain s_ no n_ ldh.
        /// </summary>
        public static string CONTAINS_NON_LDH = "Contains non-LDH characters.";

        /// <summary>
        /// The contain s_ hyphen.
        /// </summary>
        public static string CONTAINS_HYPHEN = "Leading or trailing hyphen not allowed.";

        /// <summary>
        /// The contain s_ ac e_ prefix.
        /// </summary>
        public static string CONTAINS_ACE_PREFIX = "ACE prefix (xn--) not allowed.";

        /// <summary>
        /// The to o_ long.
        /// </summary>
        public static string TOO_LONG = "String too long.";

        /// <summary>
        /// Initializes a new instance of the <see cref="IDNAException"/> class.
        /// </summary>
        /// <param name="m">
        /// The m.
        /// </param>
        public IDNAException(string m) : base(m)
        {
        }

        // TODO
        /// <summary>
        /// Initializes a new instance of the <see cref="IDNAException"/> class.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        public IDNAException(StringprepException e) : base(string.Empty, e)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDNAException"/> class.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        public IDNAException(PunycodeException e) : base(string.Empty, e)
        {
        }
    }
}