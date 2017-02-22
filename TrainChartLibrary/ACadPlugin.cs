using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

[assembly: CommandClass(typeof(TrainChartLibrary.ACadPlugin))]
namespace TrainChartLibrary
{
    public class ACadPlugin : IExtensionApplication
    {
        // как то получим это поле от пользователя
        private static string _fullFileNameWithData = "C:\\Users\\SMI\\Desktop\\TrainChartLibrary\\dataForChart.txt";

        /// <summary>
        /// Main command of the library
        /// </summary>
        [CommandMethod("GenerateTrainChart")]
        public static void GenerateTrainChart()
        {
            // Get the current document and database
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // начинаем транзакцию
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // открываем таблицу блоков документа
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                // открываем пространство модели (Model Space) - оно является одной из записей в таблице блоков документа
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // открываем таблицу слоев документа
                LayerTable acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId, OpenMode.ForWrite) as LayerTable;


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // А теперь создаем нужные мне объекты 
                ACadWorker aCadWorker = new ACadWorker(acCurDb, acTrans, acBlkTblRec, acLyrTbl);
                TrainChartGenerator trainChartGenerator = new TrainChartGenerator(aCadWorker, _fullFileNameWithData);
                trainChartGenerator.Generate();
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // фиксируем изменения
                acTrans.Commit();
            }
        }

        // функция инициализации (выполняется при загрузке плагина)
        public void Initialize()
        {
            //string greeting = "Привет! \n" +
            //                  "Я плагин для построения суточника! \n" +
            //                  "У меня есть одна команда: GenerateTrainChart \n" +
            //                  "Запусти ее для построения суточника. \n" +
            //                  "Обратите внимания, что команда потребует ввод пути к файлу с данными! \n";

            //MessageBox.Show(greeting);
            //// разберемся с wpf добавим окошечки)
        }

        // функция, выполняемая при выгрузке плагина
        public void Terminate()
        {
            //throw new NotImplementedException();
        }
    }
}
