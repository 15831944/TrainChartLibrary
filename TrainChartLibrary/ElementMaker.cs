

using System;

namespace TrainChartLibrary
{
    /// <summary>
    /// Создает элементы графика
    /// </summary>
    class ElementMaker
    {
        private ACadWorker _aCadWorker;

        /// Ордината начала строки
        /// </summary>
        private int _y;

        public ElementMaker(ACadWorker aCadWorker, int y)
        {
            _aCadWorker = aCadWorker;
            _y = y;
        }

        /// <summary>
        /// Создает отдельный элемент графика
        /// </summary>
        /// <param name="opeartionStr"></param>
        public void MakeElement(string opeartionStr)
        {
            StringParser stringParser = new StringParser();
            string[] ar = stringParser.Parse(opeartionStr, Constants.WordsInOperationSeparator);

            try
            {
                string operationType = ar[0];
                int begin = int.Parse(ar[1]);
                int duration = int.Parse(ar[2]);
                string trainNumber = ar[3];

                _aCadWorker.CreateNewLayer(trainNumber);
                _aCadWorker.MakeLayerCurrent(trainNumber);

                MakeElement(operationType, begin, _y, duration, trainNumber);
            }
            catch (IndexOutOfRangeException ignored) {} // для игнорирования лишних пробелов

            _aCadWorker.MakeLayerCurrent(Constants.DefaultZeroLayerName);
        }


        /// <summary>
        /// Ну и надо дописать этот элемент, отрисовку всех опреаций а файл нужного вида уже создавать в AnyLogic на Java
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="duration"></param>
        /// <param name="trainNumber"></param>
        private void MakeElement(string operationType, int beginX, int beginY, int duration, string trainNumber)
        {
            _aCadWorker.MakeBox(duration, 10, beginX, beginY);
        }
    }
}
