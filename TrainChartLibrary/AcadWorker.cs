
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace TrainChartLibrary
{
    /// <summary>
    /// Инкапсулирована вся работа с автокадом
    /// </summary>
    class ACadWorker
    {
        // база данных документа
        private Database _dataBase;
        // это транзакция, в которой мы создаем график
        private Transaction _transaction;
        // это пространство модели автокад
        private BlockTableRecord _model;
        // таблица слоев документа
        private LayerTable _layers;

        public ACadWorker(Database acCurDb, Transaction acTrans, BlockTableRecord acBlkTblRec, LayerTable acLyrTbl)
        {
            _dataBase = acCurDb;
            _transaction = acTrans;
            _model = acBlkTblRec;
            _layers = acLyrTbl;
        }

        /// <summary>
        /// Создает новый слой на четртеже по имени
        /// </summary>
        /// <param name="layerName"></param>
        public void CreateNewLayer(string layerName)
        {
            // создаем новый слой и задаем ему имя
            LayerTableRecord newLayer = new LayerTableRecord();
            newLayer.Name = layerName;
            // заносим созданный слой в таблицу слоев
            _layers.Add(newLayer);
            // добавляем созданный слой в документ
            _transaction.AddNewlyCreatedDBObject(newLayer, true);
        }

        /// <summary>
        /// Задает текущий слой
        /// </summary>
        /// <param name="layerName"></param>
        public void MakeLayerCurrent(string layerName)
        {
            _dataBase.Clayer = _layers[layerName];
        }

        /// <summary>
        /// Вспомогательная функция
        /// </summary>
        public void InitSolidObject()
        {
            // Create a quadrilateral (bow-tie) solid in Model space
            using (Solid ac2DSolidBow = new Solid(new Point3d(0, 0, 0),
                                            new Point3d(5, 0, 0),
                                            new Point3d(5, 8, 0),
                                            new Point3d(0, 8, 0)))
            {
                // Add the new object to the block table record and the transaction
                _model.AppendEntity(ac2DSolidBow);
                _transaction.AddNewlyCreatedDBObject(ac2DSolidBow, true);
            }
        }

        /// <summary>
        /// Создает квадрат
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void MakeBox(int width, int height)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(width, 0), 0, 0, 0);
                acPoly.AddVertexAt(2, new Point2d(width, height), 0, 0, 0);
                acPoly.AddVertexAt(3, new Point2d(0, height), 0, 0, 0);

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает квадрат с указанной толщиной линии
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="lineWeight"></param>
        public void MakeBox(int width, int height, LineWeight lineWeight)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(width, 0), 0, 0, 0);
                acPoly.AddVertexAt(2, new Point2d(width, height), 0, 0, 0);
                acPoly.AddVertexAt(3, new Point2d(0, height), 0, 0, 0);

                acPoly.LineWeight = lineWeight;

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает полилинию
        /// </summary>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void MakePolyline(int beginX, int beginY, int endX, int endY)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(beginX, beginY), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(endX, endY), 0, 0, 0);

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает полилинию с указанной толщиной линии
        /// </summary>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="lineWeight"></param>
        public void MakePolyline(int beginX, int beginY, int endX, int endY, LineWeight lineWeight)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(beginX, beginY), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(endX, endY), 0, 0, 0);

                // устанавливаем толщину линии
                acPoly.LineWeight = lineWeight;

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает полилинию с указанным типом линии
        /// </summary>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="typeName"></param>
        public void MakePolyline(int beginX, int beginY, int endX, int endY, string typeName)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(beginX, beginY), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(endX, endY), 0, 0, 0);

                // устанавливаем толщину линии
                acPoly.Linetype = typeName;

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает полилинию с указанным типом линии и масштабом
        /// </summary>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="typeName"></param>
        /// <param name="scale"></param>
        public void MakePolyline(int beginX, int beginY, int endX, int endY, string typeName, double scale)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(beginX, beginY), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(endX, endY), 0, 0, 0);

                // устанавливаем толщину линии
                acPoly.Linetype = typeName;

                // устнавливаем масштаб линии
                acPoly.LinetypeScale = scale;

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);

                // Close the polyline
                acPoly.Closed = true;
            }
        }

        /// <summary>
        /// Создает многострочный текст с указанным размером шрифта
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontSize"></param>
        /// <param name="text"></param>
        public void MakeMText(int x, int y, int fontSize, string text)
        {
            // Create a multiline text object
            using (MText acMText = new MText())
            {
                acMText.Location = new Point3d(x, y, 0);
                acMText.TextHeight = fontSize;
                acMText.Contents = text;
                

                _model.AppendEntity(acMText);
                _transaction.AddNewlyCreatedDBObject(acMText, true);
            }
        }

        /// <summary>
        /// Создает многострочный текст с указанным размером шрифта и шириной поля
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="text"></param>
        public void MakeMText(int x, int y, int fontSize, int width, string text)
        {
            // Create a multiline text object
            using (MText acMText = new MText())
            {
                acMText.Location = new Point3d(x, y, 0);
                acMText.TextHeight = fontSize; // размер шрифта
                acMText.Width = width; // ширина поля
                acMText.Contents = text;


                _model.AppendEntity(acMText);
                _transaction.AddNewlyCreatedDBObject(acMText, true);
            }
        }

        /// <summary>
        /// Задает цвет по умолчанию
        /// </summary>
        public void SetDefaultColor()
        {
            _dataBase.Cecolor = Color.FromColorIndex(ColorMethod.ByLayer, 256);
        }

        /// <summary>
        /// Задает текущий цвет
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="b3"></param>
        public void SetCurrentColor(byte b1, byte b2, byte b3)
        {
            // Set the current color
            _dataBase.Cecolor = Color.FromRgb(b1, b2, b3);
        }

        /// <summary>
        /// Загружает тип линии из "acad.lin"
        /// </summary>
        /// <param name="typeName"></param>
        public void LoadLineType(string typeName)
        {
            // Open the Linetype table for read
            LinetypeTable acLineTypTbl;
            acLineTypTbl = _transaction.GetObject(_dataBase.LinetypeTableId,
                                                OpenMode.ForRead) as LinetypeTable;

            string sLineTypName = typeName;

            if (acLineTypTbl.Has(sLineTypName) == false)
            {
                // Load the Center Linetype
                _dataBase.LoadLineTypeFile(sLineTypName, "acad.lin");
            }
        }
    }
}
