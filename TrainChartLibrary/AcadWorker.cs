
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

        public void InitLine()
        {
            // создаем линию между точками с указанными координатами
            Line acLine = new Line(new Point3d(25, 25, 0), new Point3d(33, 33, 0));

            // устанавливаем параметры созданного объекта равными параметрам по умолчанию
            acLine.SetDatabaseDefaults();

            // добавляем созданный объект в пространство модели
            _model.AppendEntity(acLine);

            // также добавляем созданный объект в транзакцию
            _transaction.AddNewlyCreatedDBObject(acLine, true);
        }

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

        public void MakeLayerCurrent(string layerName)
        {
            _dataBase.Clayer = _layers[layerName];
        }

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



        public void SetGlobalLinetypeScale(double scale)
        {
            _dataBase.Ltscale = scale;
        }

        public void SetDefaultColor()
        {
            _dataBase.Cecolor = Color.FromColorIndex(ColorMethod.ByLayer, 256);
        }

        public void SetCurrentColor(byte b1, byte b2, byte b3)
        {
            // Set the current color
            _dataBase.Cecolor = Color.FromRgb(b1, b2, b3);
        }

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
