
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
            // Open the Layer table for read
            LayerTable layerTable;
            layerTable = _transaction.GetObject(_dataBase.LayerTableId,
                                            OpenMode.ForRead) as LayerTable;

            // создаем только если слоя еще нет
            if (layerTable.Has(layerName) == false)
            {
                // создаем новый слой и задаем ему параметры
                using (LayerTableRecord newLayer = new LayerTableRecord())
                {
                    newLayer.Name = layerName;
                    // Assign the layer the ACI color 3 and a name
                    // newLayer.Color = Color.FromColorIndex(ColorMethod.ByAci, 3); // так можно задать цвет слою

                    // Upgrade the Layer table for write
                    layerTable.UpgradeOpen();

                    // Append the new layer to the Layer table and the transaction
                    layerTable.Add(newLayer);
                    _transaction.AddNewlyCreatedDBObject(newLayer, true);
                }
            }
        }

        /// <summary>
        /// Создает новый слой на четртеже по имени и задает толщину линии для слоя
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="lineWeight"></param>
        public void CreateNewLayer(string layerName, LineWeight lineWeight)
        {
            // Open the Layer table for read
            LayerTable layerTable;
            layerTable = _transaction.GetObject(_dataBase.LayerTableId,
                                            OpenMode.ForRead) as LayerTable;

            // создаем только если слоя еще нет
            if (layerTable.Has(layerName) == false)
            {
                // создаем новый слой и задаем ему параметры
                using (LayerTableRecord newLayer = new LayerTableRecord())
                {
                    newLayer.Name = layerName;
                    newLayer.LineWeight = lineWeight;

                    // Upgrade the Layer table for write
                    layerTable.UpgradeOpen();

                    // Append the new layer to the Layer table and the transaction
                    layerTable.Add(newLayer);
                    _transaction.AddNewlyCreatedDBObject(newLayer, true);
                }
            }
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
        /// Добавляет заливку по 3 точкам
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        public void MakeSolidRegion(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            // Create a quadrilateral (bow-tie) solid in Model space
            using (Solid ac2DSolidBow = new Solid(new Point3d(x1, y1, 0),
                                            new Point3d(x2, y2, 0),
                                            new Point3d(x3, y3, 0)))
            {
                // Add the new object to the block table record and the transaction
                _model.AppendEntity(ac2DSolidBow);
                _transaction.AddNewlyCreatedDBObject(ac2DSolidBow, true);
            }
        }

        /// <summary>
        /// Создает квадрат, можно указать точку вставки
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        public void MakeBox(int width, int height, int beginX = 0, int beginY = 0)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(beginX, beginY), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(beginX + width, beginY), 0, 0, 0);
                acPoly.AddVertexAt(2, new Point2d(beginX + width, beginY + height), 0, 0, 0);
                acPoly.AddVertexAt(3, new Point2d(beginX, beginY + height), 0, 0, 0);

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
        /// Строит полилинию по 3 точкам
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        public void MakePolyline(int x1, int y1, double x2, int y2, int x3, int y3)
        {
            // Create a lightweight polyline
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(x1, y1), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(x2, y2), 0, 0, 0);
                acPoly.AddVertexAt(2, new Point2d(x3, y3), 0, 0, 0);

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);
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
        /// Создает многострочный текст с указанным размером шрифта и шириной поля, и выравниванием
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fontSize"></param>
        /// <param name="width"></param>
        /// <param name="text"></param>
        /// <param name="attachmentPoint"></param>
        public void MakeMText(int x, int y, int fontSize, int width, string text, AttachmentPoint attachmentPoint)
        {
            // Create a multiline text object
            using (MText acMText = new MText())
            {
                acMText.Location = new Point3d(x, y, 0);
                acMText.TextHeight = fontSize; // размер шрифта
                acMText.Width = width; // ширина поля
                acMText.Contents = text;
                acMText.Attachment = attachmentPoint; // выравнивание


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

        private void SetGlobalLineScale(double scale)
        {
            _dataBase.Ltscale = scale;
        }

        private void SetGlobalLineScaleDefault(double scale)
        {
            _dataBase.Ltscale = scale;
        }

        public void CreateSpline()
        {
            // Create a Point3d Collection
            Point3dCollection acPt3dColl = new Point3dCollection();
            acPt3dColl.Add(new Point3d(1, 1, 0));
            acPt3dColl.Add(new Point3d(5, 5, 0));
            acPt3dColl.Add(new Point3d(10, 0, 0));

            // Set the start and end tangency
            Vector3d acStartTan = new Vector3d(0.5, 0.5, 0);
            Vector3d acEndTan = new Vector3d(0.5, 0.5, 0);

            // Create a spline
            using (Spline acSpline = new Spline(acPt3dColl,
                                            acStartTan,
                                            acEndTan, 4, 0))
            {

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acSpline);
                _transaction.AddNewlyCreatedDBObject(acSpline, true);
            }
        }

        public void MakeSpline(Point3dCollection point3DCollection)
        {
            // Set the start and end tangency
            Vector3d acStartTan = new Vector3d(0, 0, 0);
            Vector3d acEndTan = new Vector3d(0, 0, 0);

            // Create a spline
            using (Spline acSpline = new Spline(point3DCollection,
                                            acStartTan,
                                            acEndTan, 4, 0))
            {

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acSpline);
                _transaction.AddNewlyCreatedDBObject(acSpline, true);
            }
        }
    }
}
