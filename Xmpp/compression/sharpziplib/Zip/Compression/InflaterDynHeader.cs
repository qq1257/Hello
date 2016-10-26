// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="InflaterDynHeader.cs">
//   
// </copyright>
// <summary>
//   The inflater dyn header.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

using System;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    /// <summary>
    /// The inflater dyn header.
    /// </summary>
    internal class InflaterDynHeader
    {
        #region Constants

        /// <summary>
        /// The lnum.
        /// </summary>
        private const int LNUM = 0;

        /// <summary>
        /// The dnum.
        /// </summary>
        private const int DNUM = 1;

        /// <summary>
        /// The blnum.
        /// </summary>
        private const int BLNUM = 2;

        /// <summary>
        /// The bllens.
        /// </summary>
        private const int BLLENS = 3;

        /// <summary>
        /// The lens.
        /// </summary>
        private const int LENS = 4;

        /// <summary>
        /// The reps.
        /// </summary>
        private const int REPS = 5;

        /// <summary>
        /// The rep min.
        /// </summary>
        private static readonly int[] repMin = {3, 3, 11};

        /// <summary>
        /// The rep bits.
        /// </summary>
        private static readonly int[] repBits = {2, 3, 7};

        /// <summary>
        /// The b l_ order.
        /// </summary>
        private static readonly int[] BL_ORDER =
        {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};

        #endregion

        /// <summary>
        /// The decode.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="SharpZipBaseException">
        /// </exception>
        public bool Decode(StreamManipulator input)
        {
            decode_loop:
            for (;;)
            {
                switch (mode)
                {
                    case LNUM:
                        lnum = input.PeekBits(5);
                        if (lnum < 0)
                        {
                            return false;
                        }

                        lnum += 257;
                        input.DropBits(5);

// 	    System.err.println("LNUM: "+lnum);
                        mode = DNUM;
                        goto case DNUM; // fall through
                    case DNUM:
                        dnum = input.PeekBits(5);
                        if (dnum < 0)
                        {
                            return false;
                        }

                        dnum++;
                        input.DropBits(5);

// 	    System.err.println("DNUM: "+dnum);
                        num = lnum + dnum;
                        litdistLens = new byte[num];
                        mode = BLNUM;
                        goto case BLNUM; // fall through
                    case BLNUM:
                        blnum = input.PeekBits(4);
                        if (blnum < 0)
                        {
                            return false;
                        }

                        blnum += 4;
                        input.DropBits(4);
                        blLens = new byte[19];
                        ptr = 0;

// 	    System.err.println("BLNUM: "+blnum);
                        mode = BLLENS;
                        goto case BLLENS; // fall through
                    case BLLENS:
                        while (ptr < blnum)
                        {
                            int len = input.PeekBits(3);
                            if (len < 0)
                            {
                                return false;
                            }

                            input.DropBits(3);

// 		System.err.println("blLens["+BL_ORDER[ptr]+"]: "+len);
                            blLens[BL_ORDER[ptr]] = (byte) len;
                            ptr++;
                        }

                        blTree = new InflaterHuffmanTree(blLens);
                        blLens = null;
                        ptr = 0;
                        mode = LENS;
                        goto case LENS; // fall through
                    case LENS:
                    {
                        int symbol;
                        while (((symbol = blTree.GetSymbol(input)) & ~15) == 0)
                        {
                            /* Normal case: symbol in [0..15] */

                            // 		  System.err.println("litdistLens["+ptr+"]: "+symbol);
                            litdistLens[ptr++] = lastLen = (byte) symbol;

                            if (ptr == num)
                            {
                                /* Finished */
                                return true;
                            }
                        }

                        /* need more input ? */
                        if (symbol < 0)
                        {
                            return false;
                        }

                        /* otherwise repeat code */
                        if (symbol >= 17)
                        {
                            /* repeat zero */
                            // 		  System.err.println("repeating zero");
                            lastLen = 0;
                        }
                        else
                        {
                            if (ptr == 0)
                            {
                                throw new SharpZipBaseException();
                            }
                        }

                        repSymbol = symbol - 16;
                    }

                        mode = REPS;
                        goto case REPS; // fall through
                    case REPS:
                    {
                        int bits = repBits[repSymbol];
                        int count = input.PeekBits(bits);
                        if (count < 0)
                        {
                            return false;
                        }

                        input.DropBits(bits);
                        count += repMin[repSymbol];

// 	      System.err.println("litdistLens repeated: "+count);
                        if (ptr + count > num)
                        {
                            throw new SharpZipBaseException();
                        }

                        while (count-- > 0)
                        {
                            litdistLens[ptr++] = lastLen;
                        }

                        if (ptr == num)
                        {
                            /* Finished */
                            return true;
                        }
                    }

                        mode = LENS;
                        goto decode_loop;
                }
            }
        }

        /// <summary>
        /// The build lit len tree.
        /// </summary>
        /// <returns>
        /// The <see cref="InflaterHuffmanTree"/>.
        /// </returns>
        public InflaterHuffmanTree BuildLitLenTree()
        {
            var litlenLens = new byte[lnum];
            Array.Copy(litdistLens, 0, litlenLens, 0, lnum);
            return new InflaterHuffmanTree(litlenLens);
        }

        /// <summary>
        /// The build dist tree.
        /// </summary>
        /// <returns>
        /// The <see cref="InflaterHuffmanTree"/>.
        /// </returns>
        public InflaterHuffmanTree BuildDistTree()
        {
            var distLens = new byte[dnum];
            Array.Copy(litdistLens, lnum, distLens, 0, dnum);
            return new InflaterHuffmanTree(distLens);
        }

        #region Instance Fields

        /// <summary>
        /// The bl lens.
        /// </summary>
        private byte[] blLens;

        /// <summary>
        /// The bl tree.
        /// </summary>
        private InflaterHuffmanTree blTree;

        /// <summary>
        /// The blnum.
        /// </summary>
        private int blnum;

        /// <summary>
        /// The dnum.
        /// </summary>
        private int dnum;

        /// <summary>
        /// The last len.
        /// </summary>
        private byte lastLen;

        /// <summary>
        /// The litdist lens.
        /// </summary>
        private byte[] litdistLens;

        /// <summary>
        /// The lnum.
        /// </summary>
        private int lnum;

        /// <summary>
        ///     The current decode mode
        /// </summary>
        private int mode;

        /// <summary>
        /// The num.
        /// </summary>
        private int num;

        /// <summary>
        /// The ptr.
        /// </summary>
        private int ptr;

        /// <summary>
        /// The rep symbol.
        /// </summary>
        private int repSymbol;

        #endregion
    }
}