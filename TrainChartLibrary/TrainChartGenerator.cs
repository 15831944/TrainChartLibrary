
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
        private readonly ACadWorker _acadWorker; 
        // имя файла с данными для суточника
        private string _fullFileName;

        public TrainChartGenerator(ACadWorker aCadWorker, string fullFileName)
        {
            _acadWorker = aCadWorker;
            _fullFileName = fullFileName;
        }

        public ACadWorker GetACadWorker()
        {
            return _acadWorker;
        }

        public void Generate()
        {
            Parser parser = new Parser(_fullFileName);
            int rowsNumber = parser.GetAmountOfRows();

            TableGenerator tableGenerator = new TableGenerator(GetACadWorker(), rowsNumber);
            tableGenerator.Generate();

            List<string> listOfLines = parser.GetListOfLines();
            foreach (string str in listOfLines)
            {
                LineGenerator lineGenerator = new LineGenerator(GetACadWorker(), str);
                lineGenerator.Generate();
            
            }
        }

        
    }
}
