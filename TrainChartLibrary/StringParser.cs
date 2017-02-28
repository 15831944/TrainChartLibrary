
using System;
using System.Collections.Generic;

namespace TrainChartLibrary
{
    class StringParser
    {
        /// <summary>
        /// Получает список операций в строке
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public List<string> GetOperations(string line)
        {
            List<string> operationsList = new List<string>();

            string[] ar1 = line.Split(Constants.TrackNameSeparator);
            string[] ar2 = ar1[1].Split(Constants.OperationsSeparator);
            foreach (string oper in ar2)
            {
                operationsList.Add(oper);
            }
            return operationsList;
        }

        /// <summary>
        /// Получает имя пути в строке
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public string GetTrackName(string line)
        {
            string[] ar = line.Split(Constants.TrackNameSeparator);
            return ar[0];
        }

        public string[] Parse(string str, char spliter)
        {
            return str.Split(spliter);
        }
    }
}
