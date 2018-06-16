/******************************************************************************
* The MIT License
* Copyright (c) 2003 Novell Inc.  www.novell.com
* 
* Permission is hereby granted, free of charge, to any person obtaining  a copy
* of this software and associated documentation files (the Software), to deal
* in the Software without restriction, including  without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
* copies of the Software, and to  permit persons to whom the Software is 
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in 
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*******************************************************************************/
//
// Novell.Directory.Ldap.Asn1.Asn1Length.cs
//
// Author:
//   Sunil Kumar (Sunilk@novell.com)
//
// (C) 2003 Novell, Inc (http://www.novell.com)
//

using System;
using System.IO;

namespace Novell.Directory.Ldap.Asn1
{
    /// <summary>
    ///     This class provides a means to manipulate ASN.1 Length's. It will
    ///     be used by Asn1Encoder's and Asn1Decoder's by composition.
    /// </summary>
    [CLSCompliant(true)]
    public sealed class Asn1Length
    {
        /// <summary> Returns the length of this Asn1Length.</summary>
        public int Length => _length;

        /// <summary> Returns the encoded length of this Asn1Length.</summary>
        public int EncodedLength => _encodedLength;

        /* Private variables
        */

        private int _length;
        private int _encodedLength;

        /* Constructors for Asn1Length
        */

        /// <summary> Constructs an empty Asn1Length.  Values are added by calling reset</summary>
        public Asn1Length()
        {
        }

        /// <summary> Constructs an Asn1Length</summary>
        public Asn1Length(int length)
        {
            _length = length;
        }

        /// <summary>
        ///     Constructs an Asn1Length object by decoding data from an
        ///     input stream.
        /// </summary>
        /// <param name="in">
        ///     A byte stream that contains the encoded ASN.1
        /// </param>
        public Asn1Length(Stream inRenamed)
        {
            var r = inRenamed.ReadByte();
            _encodedLength++;
            if (r == 0x80)
                _length = -1;
            else if (r < 0x80)
                _length = r;
            else
            {
                _length = 0;
                for (r = r & 0x7F; r > 0; r--)
                {
                    var part = inRenamed.ReadByte();
                    _encodedLength++;
                    if (part < 0)
                        throw new EndOfStreamException("BERDecoder: decode: EOF in Asn1Length");
                    _length = (_length << 8) + part;
                }
            }
        }

        /// <summary>
        ///     Resets an Asn1Length object by decoding data from an
        ///     input stream.
        ///     Note: this was added for optimization of Asn1.LBERdecoder.decode()
        /// </summary>
        /// <param name="in">
        ///     A byte stream that contains the encoded ASN.1
        /// </param>
        public void Reset(Stream inRenamed)
        {
            _encodedLength = 0;
            var r = inRenamed.ReadByte();
            _encodedLength++;
            if (r == 0x80)
                _length = -1;
            else if (r < 0x80)
                _length = r;
            else
            {
                _length = 0;
                for (r = r & 0x7F; r > 0; r--)
                {
                    var part = inRenamed.ReadByte();
                    _encodedLength++;
                    if (part < 0)
                        throw new EndOfStreamException("BERDecoder: decode: EOF in Asn1Length");
                    _length = (_length << 8) + part;
                }
            }
        }
    }
}