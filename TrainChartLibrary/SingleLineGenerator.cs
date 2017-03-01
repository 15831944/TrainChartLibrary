
using System.Collections.Generic;

namespace TrainChartLibrary
{
    class SingleLineGenerator : IGenerator
    {
        /// <summary>
        /// Это информация о строчке суточника (пути)
        /// </summary>
        private string _line;
        private ACadWorker _aCadWorker;
        /// <summary>
        /// Ордината начала строки
        /// </summary>
        private int _y;

        public SingleLineGenerator(ACadWorker aCadWorker, string line, int y)
        {
            _aCadWorker = aCadWorker;
            _line = line;
            _y = y;
        }

        /// <summary>
        /// Создает элементы в строчке графика
        /// </summary>
        public void Generate()
        {
            StringParser stringParser = new StringParser();

            string trackName = stringParser.GetTrackName(_line); // получаем название строчки (пути)
            _aCadWorker.MakeLayerCurrent(Constants.TableLayerName);
            _aCadWorker.MakeMText(-Constants.LengthOfTrackName + 8, _y + Constants.HeightOfNumbers + 5, Constants.HeightOfNumbers, trackName);
            _aCadWorker.MakeLayerCurrent(Constants.DefaultZeroLayerName);

            List<string> operations = stringParser.GetOperations(_line); // получаем элементы графика в строке
            ElementMaker elementMaker = new ElementMaker(_aCadWorker, _y);
            // создаем каждый элемент
            foreach (string oper in operations)
            {
                elementMaker.MakeElement(oper);
            }
            elementMaker.PrintDict();
        }
    }
}
