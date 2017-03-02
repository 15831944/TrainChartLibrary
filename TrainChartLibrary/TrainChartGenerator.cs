
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace TrainChartLibrary 
{
    /// <summary>
    /// Создает суточник, используя 
    /// Parser - для работы с файлом и строками
    /// TableGenerator - создает таблицу для графика 
    /// SingleLineGenerator - создает отдельную строку графика
    /// </summary>
    class TrainChartGenerator : IGenerator
    {
        // Глобальный словарь для хранения точек блоков движения для соединительных линий
        // Словарь здесь, т.к. для каждой строки суточника создается свой SingleLineCreator
        public static Dictionary<string, Point3d> LastTrainPointDictionary = new Dictionary<string, Point3d>(); // храним последнюю коорденату блока движения поезда

        private readonly ACadWorker _acadWorker; 
        // имя файла с данными для суточника
        private readonly string _fullFileName;

        public TrainChartGenerator(ACadWorker aCadWorker, string fullFileName)
        {
            _acadWorker = aCadWorker;
            _fullFileName = fullFileName;
        }

        public ACadWorker GetACadWorker()
        {
            return _acadWorker;
        }

        /// <summary>
        /// Создает суточник
        /// </summary>
        public void Generate()
        {
            FileParser parser = new FileParser(_fullFileName); // Парсим файл
            int rowsNumber = parser.GetAmountOfRows(); // Получаем количество строк (путей)

            // Делаем таблицу для суточника
            TableGenerator tableGenerator = new TableGenerator(GetACadWorker(), rowsNumber);
            tableGenerator.Generate();

            // Генерируем каждую строчку суточника
            List<string> listOfLines = parser.GetListOfLines();
            int y = (rowsNumber - 1)*Constants.HeightOfRow; // начинаем заполнять с первой строки (y - ординита строки)
            foreach (string str in listOfLines)
            {
                SingleLineGenerator lineGenerator = new SingleLineGenerator(GetACadWorker(), str, y);
                lineGenerator.Generate();
                y -= Constants.HeightOfRow;
            }
        }
    }
}
