
using System.Collections.Generic;
using System;
using System.Collections;

namespace TrainChartLibrary
{
    class FileParser
    {
        private readonly int _amountOfRows;
        private readonly List<string> _listOfLines = new List<string>();

        public FileParser(string fileName)
        {
            string line;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                _listOfLines.Add(line);
                _amountOfRows++;
            }

            file.Close();
        }

        /// <summary>
        /// Возвращает количество строк в файле
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfRows()
        {
            return _amountOfRows;
        }

        /// <summary>
        /// Возвращает список строк для суточника
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfLines()
        {
            return _listOfLines;
        }
    }
}
