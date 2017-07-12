using System;
using System.Net;

namespace Sharp7
{
    public class WrappedS7
    {
        private readonly S7Client _client;

        public int LastErrorCode => _client.LastError();

        public string LastErrorString => _client.ErrorText(_client.LastError());


        public WrappedS7()
        {
            _client = new S7Client();
        }


        public int ConnectTo(IPAddress address, int rack, int slot) => ConnectTo(address.ToString(), rack, slot);
        public int ConnectTo(string address, int rack, int slot)
        {
            return _client.ConnectTo(address, rack, slot);
        }

        public object Read(DataType area, int database, int startAddress, int amount, VarType type)
        {
            var recievedBytes = new byte[RecievedDataLength(type, amount)];
            _client.ReadArea((int)area, database, startAddress, amount, VarTypeToS7(type), recievedBytes);
            return ParseBytes(type, recievedBytes, amount);
        }

        public int Write(DataType area, int database, int startAddress, byte[] value)
        {
            return _client.WriteArea((int) area, database, startAddress, value.Length, 0x02, value);
        }


        private int RecievedDataLength(VarType type, int amount)
        {
            return S7.DataSizeByte(VarTypeToS7(type)) * amount;
        }

        /// <summary>
        /// Given a S7 variable type (Bool, Word, DWord, etc.), it converts the bytes in the appropriate C# format.
        /// </summary>
        /// <param name="varType"></param>
        /// <param name="bytes"></param>
        /// <param name="varCount"></param>
        /// <returns></returns>
        private object ParseBytes(VarType varType, byte[] bytes, int varCount)
        {
            if (bytes == null) return null;

            switch (varType)
            {
                case VarType.Byte:
                    if (varCount == 1)
                        return bytes[0];
                    else
                        return bytes;
                case VarType.Word:
                    if (varCount == 1)
                        return Types.Word.FromByteArray(bytes);
                    else
                        return Types.Word.ToArray(bytes);
                case VarType.Int:
                    if (varCount == 1)
                        return Types.Int.FromByteArray(bytes);
                    else
                        return Types.Int.ToArray(bytes);
                case VarType.DWord:
                    if (varCount == 1)
                        return Types.DWord.FromByteArray(bytes);
                    else
                        return Types.DWord.ToArray(bytes);
                case VarType.DInt:
                    if (varCount == 1)
                        return Types.DInt.FromByteArray(bytes);
                    else
                        return Types.DInt.ToArray(bytes);
                case VarType.Real:
                    if (varCount == 1)
                        return Types.Double.FromByteArray(bytes);
                    else
                        return Types.Double.ToArray(bytes);
                case VarType.String:
                    return Types.String.FromByteArray(bytes);
                case VarType.Timer:
                    if (varCount == 1)
                        return Types.Timer.FromByteArray(bytes);
                    else
                        return Types.Timer.ToArray(bytes);
                case VarType.Counter:
                    if (varCount == 1)
                        return Types.Counter.FromByteArray(bytes);
                    else
                        return Types.Counter.ToArray(bytes);
                case VarType.Bit:
                    return null; //TODO
                default:
                    return null;
            }
        }

        private int VarTypeToS7(VarType type)
        {
            switch (type)
            {
                case VarType.Bit:
                    return S7Consts.S7WLBit;
                case VarType.Byte:
                    return S7Consts.S7WLByte;
                case VarType.Word:
                    return S7Consts.S7WLWord;
                case VarType.DWord:
                    return S7Consts.S7WLDWord;
                case VarType.Real:
                    return S7Consts.S7WLReal;
                case VarType.Counter:
                    return S7Consts.S7WLCounter;
                case VarType.Timer:
                    return S7Consts.S7WLTimer;
                case VarType.Int:
                    return S7Consts.S7WLWord;
                case VarType.DInt:
                    return S7Consts.S7WLDWord;
                case VarType.Char:
                    return S7Consts.S7WLByte;
                case VarType.String:
                    return S7Consts.S7WLByte;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
