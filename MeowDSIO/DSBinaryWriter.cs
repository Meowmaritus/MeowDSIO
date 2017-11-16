using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO
{
    public class DSBinaryWriter : BinaryWriter
    {
        private static Encoding ShiftJISEncoding = Encoding.GetEncoding("shift_jis");

        public DSBinaryWriter(Stream output) : base(output) { }
        public long Position => BaseStream.Position;
        public long Length => BaseStream.Length;
        public void Goto(long absoluteOffset) => BaseStream.Seek(absoluteOffset, SeekOrigin.Begin);
        public void Jump(long relativeOffset) => BaseStream.Seek(relativeOffset, SeekOrigin.Current);
        private Stack<long> StepStack = new Stack<long>();

        public bool BigEndian = false;

        public char StrEscapeChar = (char)0;

        public void StepIn(long offset)
        {
            StepStack.Push(Position);
            Goto(offset);
        }

        public void StepOut()
        {
            Goto(StepStack.Pop());
        }

        public void DoAt(long offset, Action doAction)
        {
            StepIn(offset);
            doAction();
            StepOut();
        }

        #region Endianness

        private byte[] PrepareBytes(byte[] b)
        {
            //Check if the BitConverter is expecting little endian
            if (BitConverter.IsLittleEndian)
            {
                //It's expecting little endian so we must reverse our big endian bytes
                Array.Reverse(b);
            }
            return b;
        }

        private void WritePreparedBytes(byte[] b)
        {
            base.Write(PrepareBytes(b));
        }

        public override void Write(char[] chars)
        {
            if (!BigEndian)
            {
                base.Write(chars);
                return;
            }

            for (int i = 0; i < chars.Length; i++)
            {
                WritePreparedBytes(BitConverter.GetBytes(chars[i]));
            }
        }
        public override void Write(long value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(uint value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(int value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(ushort value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(short value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(decimal value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            int[] chunks = Decimal.GetBits(value);
            byte[] bytes = new byte[16];

            for (int i = 0; i < 16; i += 4)
            {
                byte[] b = BitConverter.GetBytes(chunks[i / 4]);
                for (int j = 0; j < 4; j++)
                {
                    bytes[i + j] = b[j];
                }
            }

            WritePreparedBytes(bytes);
        }
        public override void Write(double value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(float value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }
        public override void Write(char ch)
        {
            if (!BigEndian)
            {
                base.Write(ch);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(ch));
        }
        private new void Write(string value)
        {
            //Make generic string write method unavailable to the outside world.
            //Caller must instead call one of the more specific string write methods.
        }
        public override void Write(ulong value)
        {
            if (!BigEndian)
            {
                base.Write(value);
                return;
            }

            WritePreparedBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an ASCII string directly without padding or truncating it.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// <param name="terminate">Whether to append a string terminator character of value 0 to the end of the written string.</param>
        public void WriteStringAscii(string str, bool terminate)
        {
            byte[] valueBytes = new byte[terminate ? str.Length + 1 : str.Length];
            Encoding.ASCII.GetBytes(str, 0, str.Length, valueBytes, 0);
            Write(valueBytes);
        }

        /// <summary>
        /// Writes a Shift-JIS string.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// /// <param name="terminate">Whether to append a string terminator character of value 0 to the end of the written string.</param>
        public void WriteStringShiftJIS(string str, bool terminate)
        {
            byte[] b = ShiftJISEncoding.GetBytes(str);
            if (terminate)
                Array.Resize(ref b, b.Length + 1);

            Write(b);
        }

        #endregion

    }
}
