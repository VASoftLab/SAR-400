using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostumeController
{
    public enum DataType { _byte, _int16, _int32, _float }

    public class PackedData
    {
        public int index = -1;
        public DataType type = DataType._int16;
        public float Value
        {
            get { return _value; }
            set
            {
                switch (type)
                {
                    case DataType._byte:
                        _value = (byte)Utility.Round(value);
                        break;

                    case DataType._int16:
                        _value = (Int16)Utility.Round(value);
                        break;

                    case DataType._int32:
                        _value = Utility.Round(value);
                        break;

                    default:
                        _value = value;
                        break;
                }
            }
        }

        private float _value;

        public void ReadValue(byte[] array)
        {
            if (array == null || index < 0 || index >= array.Length)
                return;

            switch (type)
            {
                case DataType._byte:
                    _value = array[index];
                    break;
                case DataType._int16:
                    _value = BitConverter.ToInt16(array, index);
                    break;
                case DataType._int32:
                    _value = BitConverter.ToInt32(array, index);
                    break;
                case DataType._float:
                    _value = BitConverter.ToSingle(array, index);
                    break;
            }
        }

        public void WriteValue(ref byte[] array)
        {
            if (array == null || index < 0 || index >= array.Length)
                return;

            switch (type)
            {
                case DataType._byte:
                    array[index] = (byte)Utility.Round(_value);
                    break;

                case DataType._int16:
                    var b16 = BitConverter.GetBytes((Int16)Utility.Round(_value));
                    array[index] = b16[0];
                    array[index + 1] = b16[1];
                    break;

                case DataType._int32:
                    var b32 = BitConverter.GetBytes(Utility.Round(_value));
                    array[index] = b32[0];
                    array[index + 1] = b32[1];
                    array[index + 2] = b32[2];
                    array[index + 3] = b32[3];
                    break;

                case DataType._float:
                    var f32 = BitConverter.GetBytes(_value);
                    array[index] = f32[0];
                    array[index + 1] = f32[1];
                    array[index + 2] = f32[2];
                    array[index + 3] = f32[3];
                    break;
            }
        }
    }
}
