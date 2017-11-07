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
        protected abstract void Read(DSBinaryReader bin);
        protected abstract void Write(DSBinaryWriter bin);

        public static T LoadFromFile<T>(string filePath)
            where T : DataFile, new()
        {
            using (var fileStream = File.Open(filePath, FileMode.Open))
            {
                using (var binaryReader = new DSBinaryReader(fileStream))
                {
                    T result = new T();

                    try
                    {
                        result.Read(binaryReader);
                    }
                    catch (LoadAbortedException)
                    {
                        throw new LoadAbortedException(filePath);
                    }
                    
                    return result;
                }
            }
        }

        public static void SaveToFile<T>(T data, string filePath)
            where T : DataFile, new()
        {
            using (var fileStream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                fileStream.SetLength(0);
                using (var binaryWriter = new DSBinaryWriter(fileStream))
                {
                    data.Write(binaryWriter);
                }
            }
        }

    }
}
