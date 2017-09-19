using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using UnityEngine;

namespace PlatForm.Utilities
{
    public enum SerializedType : ushort
    {
        ByteArray = 0,
        Object = 1,
        String = 2,
        Datetime = 3,
        Bool = 4,
        //SByte     = 5, //Makes no sense.
        Byte = 6,
        Short = 7,
        UShort = 8,
        Int = 9,
        UInt = 10,
        Long = 11,
        ULong = 12,
        Float = 13,
        Double = 14,

        CompressedByteArray = 255,
        CompressedObject = 256,
        CompressedString = 257,
    }

    public class SerializeHelper
    {
        public SerializeHelper()
        { }

        #region XML序列化
        /// <summary>
        /// 文件化XML序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        /// 文本化XML序列化
        /// </summary>
        /// <param name="item">对象</param>
        public string ToXml<T>(T item)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        /// 文本化XML反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public T FromXml<T>(string str)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        #endregion        

        #region BinaryFormatter序列化
        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToBinary<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static byte[] ToBinaryByte<T>(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }

        /// <summary>
        /// BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T FromBinary<T>(string str)
        {
            int intLen = str.Length / 2;
            byte[] bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion


        public static void SaveToFile<T>(T item, string path, FileMode fileMode)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                SurrogateSelector ss = new SurrogateSelector();
                Assets.Editor.Vector3SerializationSurrogate v3Surrogate = new Assets.Editor.Vector3SerializationSurrogate();
                ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
                formatter.SurrogateSelector = ss;
                using (FileStream fs = new FileStream(path, fileMode))
                {
                    formatter.Serialize(fs, item);
                    fs.Close();
                }
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to serialize. Reason: " + e.Message);
                throw;
            }
        }

        public static T loadFromFile<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            Assets.Editor.Vector3SerializationSurrogate v3Surrogate = new Assets.Editor.Vector3SerializationSurrogate();
            ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
            formatter.SurrogateSelector = ss;
            T t;
            try
            {
                t = (T)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Debug.Log("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return t;
        }


        /// <summary>
        /// 将对象序列化为二进制字节
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <returns></returns>
        public static byte[] SerializeToBinary(object obj)
        {
            byte[] bytes = new byte[2500];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(memoryStream, obj);
                memoryStream.Seek(0, 0);

                if (memoryStream.Length > bytes.Length)
                {
                    bytes = new byte[memoryStream.Length];
                }
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// 从二进制字节中反序列化为对象
        /// </summary>
        /// <param name="type">对象的类型</param>
        /// <param name="bytes">字节数组</param>
        /// <returns>反序列化后得到的对象</returns>
        public static object DeserializeFromBinary(Type type, byte[] bytes)
        {
            object result = new object();
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                result = serializer.Deserialize(memoryStream);
            }

            return result;
        }

        /// <summary>
        /// 将文件对象序列化到文件中
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="path">文件路径</param>
        /// <param name="fileMode">文件打开模式</param>
        public static void SerializeToBinary(object obj, string path, FileMode fileMode)
        {
            using (FileStream fs = new FileStream(path, fileMode))
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// 将文件对象序列化到文件中
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="path">文件路径</param>
        public static void SerializeToBinary(object obj, string path)
        {
            SerializeToBinary(obj, path, FileMode.Create);
        }

        /// <summary>
        /// 从二进制文件中反序列化为对象
        /// </summary>
        /// <param name="type">对象的类型</param>
        /// <param name="path">二进制文件路径</param>
        /// <returns>反序列化后得到的对象</returns>
        public static object DeserializeFromBinary(Type type, string path)
        {
            object result = new object();
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                result = serializer.Deserialize(fileStream);
            }

            return result;
        }

        /// <summary>
        /// 获取对象的转换为二进制的字节大小
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetByteSize(object obj)
        {
            long result;
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                bFormatter.Serialize(stream, obj);
                result = stream.Length;
            }
            return result;
        }

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <param name="obj">待克隆的对象</param>
        /// <returns>克隆的一个新的对象</returns>
        public static object Clone(object obj)
        {
            object cloned = null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    bFormatter.Serialize(memoryStream, obj);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    cloned = bFormatter.Deserialize(memoryStream);
                }
                catch //(Exception e)
                {
                    ;
                }
            }

            return cloned;
        }

        /// <summary>
        /// 从文件中读取文本内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的内容</returns>
        public static string ReadFile(string path)
        {
            string content = string.Empty;
            using (StreamReader reader = new StreamReader(path))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }

        public static byte[] Serialize(object value, out SerializedType type, uint compressionThreshold)
        {
            byte[] bytes;
            if (value is byte[])
            {
                bytes = (byte[])value;
                type = SerializedType.ByteArray;
                if (bytes.Length > compressionThreshold)
                {
                    bytes = compress(bytes);
                    type = SerializedType.CompressedByteArray;
                }
            }
            else if (value is string)
            {
                bytes = Encoding.UTF8.GetBytes((string)value);
                type = SerializedType.String;
                if (bytes.Length > compressionThreshold)
                {
                    bytes = compress(bytes);
                    type = SerializedType.CompressedString;
                }
            }
            else if (value is DateTime)
            {
                bytes = BitConverter.GetBytes(((DateTime)value).Ticks);
                type = SerializedType.Datetime;
            }
            else if (value is bool)
            {
                bytes = new byte[] { (byte)((bool)value ? 1 : 0) };
                type = SerializedType.Bool;
            }
            else if (value is byte)
            {
                bytes = new byte[] { (byte)value };
                type = SerializedType.Byte;
            }
            else if (value is short)
            {
                bytes = BitConverter.GetBytes((short)value);
                type = SerializedType.Short;
            }
            else if (value is ushort)
            {
                bytes = BitConverter.GetBytes((ushort)value);
                type = SerializedType.UShort;
            }
            else if (value is int)
            {
                bytes = BitConverter.GetBytes((int)value);
                type = SerializedType.Int;
            }
            else if (value is uint)
            {
                bytes = BitConverter.GetBytes((uint)value);
                type = SerializedType.UInt;
            }
            else if (value is long)
            {
                bytes = BitConverter.GetBytes((long)value);
                type = SerializedType.Long;
            }
            else if (value is ulong)
            {
                bytes = BitConverter.GetBytes((ulong)value);
                type = SerializedType.ULong;
            }
            else if (value is float)
            {
                bytes = BitConverter.GetBytes((float)value);
                type = SerializedType.Float;
            }
            else if (value is double)
            {
                bytes = BitConverter.GetBytes((double)value);
                type = SerializedType.Double;
            }
            else
            {
                //Object
                using (MemoryStream ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, value);
                    bytes = ms.GetBuffer();
                    type = SerializedType.Object;
                    if (bytes.Length > compressionThreshold)
                    {
                        bytes = compress(bytes);
                        type = SerializedType.CompressedObject;
                    }
                }
            }
            return bytes;
        }

        private static byte[] compress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream gzs = new DeflateStream(ms, CompressionMode.Compress, false))
                {
                    gzs.Write(bytes, 0, bytes.Length);
                }
                ms.Close();
                return ms.GetBuffer();
            }
        }

        private static byte[] decompress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes, false))
            {
                using (DeflateStream gzs = new DeflateStream(ms, CompressionMode.Decompress, false))
                {
                    using (MemoryStream dest = new MemoryStream())
                    {
                        byte[] tmp = new byte[bytes.Length];
                        int read;
                        while ((read = gzs.Read(tmp, 0, tmp.Length)) != 0)
                        {
                            dest.Write(tmp, 0, read);
                        }
                        dest.Close();
                        return dest.GetBuffer();
                    }
                }
            }
        }

        public static object DeSerialize(byte[] bytes, SerializedType type)
        {
            switch (type)
            {
                case SerializedType.String:
                    return Encoding.UTF8.GetString(bytes);
                case SerializedType.Datetime:
                    return new DateTime(BitConverter.ToInt64(bytes, 0));
                case SerializedType.Bool:
                    return bytes[0] == 1;
                case SerializedType.Byte:
                    return bytes[0];
                case SerializedType.Short:
                    return BitConverter.ToInt16(bytes, 0);
                case SerializedType.UShort:
                    return BitConverter.ToUInt16(bytes, 0);
                case SerializedType.Int:
                    return BitConverter.ToInt32(bytes, 0);
                case SerializedType.UInt:
                    return BitConverter.ToUInt32(bytes, 0);
                case SerializedType.Long:
                    return BitConverter.ToInt64(bytes, 0);
                case SerializedType.ULong:
                    return BitConverter.ToUInt64(bytes, 0);
                case SerializedType.Float:
                    return BitConverter.ToSingle(bytes, 0);
                case SerializedType.Double:
                    return BitConverter.ToDouble(bytes, 0);
                case SerializedType.Object:
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        return new BinaryFormatter().Deserialize(ms);
                    }
                case SerializedType.CompressedByteArray:
                    return DeSerialize(decompress(bytes), SerializedType.ByteArray);
                case SerializedType.CompressedString:
                    return DeSerialize(decompress(bytes), SerializedType.String);
                case SerializedType.CompressedObject:
                    return DeSerialize(decompress(bytes), SerializedType.Object);
                case SerializedType.ByteArray:
                default:
                    return bytes;
            }
        }
    }
}
