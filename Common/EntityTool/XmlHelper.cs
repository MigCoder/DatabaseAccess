using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.EntityTool
{
    /// <summary>
    /// 实体类XML序列化映射类
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// 从XML文本中读取出一个实体
        /// </summary>
        /// <param name="xml">XML文本</param>
        /// <param name="encoding">编码方式</param>
        public static T ReadFromXml<T>(string xml, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentNullException();
            if (encoding == null)
                throw new ArgumentNullException();

            var mapper = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream(encoding.GetBytes(xml)))
            {
                using (var sr = new StreamReader(ms, encoding))
                {
                    return (T)mapper.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 从XML文件中读取出一个实体
        /// </summary>
        /// <param name="xmlPath">XML路径</param>
        /// <param name="encoding">编码方式</param>
        public static T ReadFromFile<T>(string xmlPath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlPath))
                throw new ArgumentNullException("xmlPath");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(xmlPath, encoding);
            return ReadFromXml<T>(xml, encoding);
        }

        /// <summary>
        /// 将实体映射到XML文件
        /// </summary>
        /// <param name="data">实体内容</param>
        /// <param name="path">XML路径</param>
        /// <param name="encoding">编码方式</param>
        public static void MappingToFile(object data, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                WriteFile(file, data, encoding);
            }
        }

        /// <summary>
        /// 将实体类映射成XML字符串
        /// </summary>
        /// <param name="data">实体内容</param>
        /// <param name="encoding">编码方式</param>
        public static string MappingToXml(object data, Encoding encoding)
        {
            using (var stream = new MemoryStream())
            {
                WriteFile(stream, data, encoding);

                stream.Position = 0;
                using (var reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void WriteFile(Stream stream, object data, Encoding encoding)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var serializer = new XmlSerializer(data.GetType());

            var settings = new XmlWriterSettings
                {
                    Indent = true,
                    NewLineChars = Environment.NewLine,
                    Encoding = encoding,
                    IndentChars = "    "
                };

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, data);
                writer.Close();
            }
        }
    }
}
