using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO
{
    public abstract class DataFile
    {
        protected abstract void Read(BinaryReader bin);
        protected abstract void Write(BinaryWriter bin);

        public static T LoadFromFile<T>(string filePath)
            where T : DataFile, new()
        {
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    T result = new T();
                    result.Read(binaryReader);
                    return result;
                }
            }
        }

        public static void SaveToFile<T>(T data, string filePath)
            where T : DataFile, new()
        {
            using (var fileStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {
                    data.Write(binaryWriter);
                }
            }
        }

    }
}
