
using System.IO;
using System.Text;

namespace TrainChartLibrary
{
    class DataFileParser
    {
        private string _fileName;

        public DataFileParser(string fileName)
        {
            _fileName = fileName;
        }

        public string GetFileContact()
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (StreamReader streamReader = new StreamReader(_fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    stringBuilder.Append(streamReader.ReadLine());
                }
            }
            return stringBuilder.ToString();
        }
    }
}
