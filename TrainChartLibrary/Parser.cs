
using System.Collections.Generic;
using System;
using System.Collections;

namespace TrainChartLibrary
{
    class Parser
    {
        private string _fileName;

        public Parser(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Возвращает количество строк в файле
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfRows()
        {
            return 20;
        }

        /// <summary>
        /// Возвращает список строк для суточника
        /// </summary>
        /// <returns></returns>
        public List<string> GetListOfLines()
        {
            return new List<string>();
        }
    }
}
