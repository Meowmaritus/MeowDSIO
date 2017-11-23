using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO
{
    public class DSBinaryReader : BinaryReader
    {
        private static Encoding ShiftJISEncoding = Encoding.GetEncoding("shift_jis");

        public DSBinaryReader(Stream input) : base(input) { }
        public long Position { get => BaseStream.Position; set => BaseStream.Position = value; }
        public long Length => BaseStream.Length;
        public void Goto(long absoluteOffset) => BaseStream.Seek(absoluteOffset, SeekOrigin.Begin);
        public void Jump(long relativeOffset) => BaseStream.Seek(relativeOffset, SeekOrigin.Current);
        private Stack<long> StepStack = new Stack<long>();

        public bool BigEndian = false;

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

        private byte[] GetPreparedBytes(int count)
        {
            byte[] b = base.ReadBytes(count);
            return PrepareBytes(b);
        }

        public override char ReadChar()
        {
            if (!BigEndian)
                return base.ReadChar();

            return BitConverter.ToChar(GetPreparedBytes(2), 0);
        }
        public override char[] ReadChars(int count)
        {
            if (!BigEndian)
                return base.ReadChars(count);

            char[] chr = new char[count];

            for (int i = 0; i < count; i++)
            {
                chr[i] = BitConverter.ToChar(GetPreparedBytes(2), 0);
            }

            return chr;
        }
        public override decimal ReadDecimal()
        {
            if (!BigEndian)
                return base.ReadDecimal();

            byte[] b = GetPreparedBytes(16);
            int[] chunks = new int[4];

            for (int i = 0; i < 16; i += 4)
            {
                chunks[i / 4] = BitConverter.ToInt32(b, i);
            }

            return new decimal(chunks);
        }
        public override double ReadDouble()
        {
            if (!BigEndian)
                return base.ReadDouble();

            return BitConverter.ToDouble(GetPreparedBytes(8), 0);
        }
        public override short ReadInt16()
        {
            if (!BigEndian)
                return base.ReadInt16();

            return BitConverter.ToInt16(GetPreparedBytes(2), 0);
        }
        public override int ReadInt32()
        {
            if (!BigEndian)
                return base.ReadInt32();

            return BitConverter.ToInt32(GetPreparedBytes(4), 0);
        }
        public override long ReadInt64()
        {
            if (!BigEndian)
                return base.ReadInt64();

            return BitConverter.ToInt64(GetPreparedBytes(8), 0);
        }
        public override float ReadSingle()
        {
            if (!BigEndian)
                return base.ReadSingle();

            return BitConverter.ToSingle(GetPreparedBytes(4), 0);
        }
        public override ushort ReadUInt16()
        {
            if (!BigEndian)
                return base.ReadUInt16();

            return BitConverter.ToUInt16(GetPreparedBytes(2), 0);
        }
        public override uint ReadUInt32()
        {
            if (!BigEndian)
                return base.ReadUInt32();

            return BitConverter.ToUInt32(GetPreparedBytes(4), 0);
        }
        public override ulong ReadUInt64()
        {
            if (!BigEndian)
                return base.ReadUInt64();

            return BitConverter.ToUInt64(GetPreparedBytes(4), 0);
        }
        #endregion


        /// <summary>
        /// Reads an ASCII string.
        /// </summary>
        /// <param name="length">If non-null, reads the specified number of characters. 
        /// <para/>If null, reads characters until it reaches a control character of value 0 (and this 0-value is excluded from the returned string).</param>
        /// <returns>An ASCII string.</returns>
        public string ReadStringAscii(int? length = null)
        {
            if (length.HasValue)
            {
                return Encoding.ASCII.GetString(ReadBytes(length.Value));
            }
            else
            {
                var sb = new StringBuilder();

                byte[] nextByte = new byte[] { 0 };

                while (true)
                {
                    nextByte[0] = ReadByte();

                    if (nextByte[0] > 0)
                        sb.Append(Encoding.ASCII.GetChars(nextByte));
                    else
                        break;
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Reads a Shift-JIS string.
        /// </summary>
        /// <returns>A Shift-JIS string.</returns>
        public string ReadStringShiftJIS()
        {
            List<byte> shiftJisData = new List<byte>();

            byte nextByte = 0;

            while (true)
            {
                nextByte = ReadByte();

                if (nextByte > 0)
                    shiftJisData.Add(nextByte);
                else
                    break;
            }

            return ShiftJISEncoding.GetString(shiftJisData.ToArray());
        }

        public string ReadStringShiftJIS(int specificLength)
        {
            return ShiftJISEncoding.GetString(ReadBytes(specificLength).ToArray());
        }


        public void Pad(int align)
        {
            var off = Position % align;
            if (off > 0)
            {
                ReadBytes((int)(align - off));
            }
        }

        public byte ReadDelimiter()
        {
            byte result = ReadByte();
            Pad(4);
            return result;
        }

        public Vector2 ReadVector2()
        {
            float x = ReadSingle();
            float y = ReadSingle();
            return new Vector2(x, y);
        }

        public Vector3 ReadVector3()
        {
            float x = ReadSingle();
            float y = ReadSingle();
            float z = ReadSingle();
            return new Vector3(x, y, z);
        }

        public Vector4 ReadVector4()
        {
            float w = ReadSingle();
            float x = ReadSingle();
            float y = ReadSingle();
            float z = ReadSingle();
            return new Vector4(w, x, y, z);
        }

        public string ReadMtdName(out byte delim)
        {
            int valLength = ReadInt32();
            string result = ReadStringShiftJIS(valLength);
            delim = ReadDelimiter();
            return result;
        }

        public string ReadMtdName()
        {
            return ReadMtdName(out _);
        }

        public byte ReadByte(string valName, params byte[] checkValues)
        {
            byte val = ReadByte();
            if (!checkValues.Contains(val))
            {
                throw new Exception($"Unexpected value found for {valName}: {val}");
            }
            return val;
        }
    }
}
