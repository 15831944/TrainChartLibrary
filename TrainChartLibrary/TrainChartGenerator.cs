
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace TrainChartLibrary
{
    class TrainChartGenerator
    {
        // база данных документа
        private Database _dataBase;
        // это транзакция, в которой мы создаем график
        private Transaction _transaction;
        // это пространство модели автокад
        private BlockTableRecord _model;
        // таблица слоев документа
        private LayerTable _layers;

        // имя файла с данными для суточника
        private string _fullFileName;

        public TrainChartGenerator(Database acCurDb, Transaction acTrans, BlockTableRecord acBlkTblRec, LayerTable acLyrTbl, string fullFileName)
        {
            _dataBase = acCurDb;
            _transaction = acTrans;
            _model = acBlkTblRec;
            _fullFileName = fullFileName;
            _layers = acLyrTbl;
        }

        public void Generate()
        {
            DataFileParser dataFileParser = new DataFileParser(_fullFileName);
            string fileContant = dataFileParser.GetFileContact();
            MessageBox.Show("Информация из переданного файла: \n" + fileContant);

            InitTable(0);
            //InitSolidObject();
            InitPolyLine();
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

        public void InitTable(int rowNumber)
        {
            CreateNewLayer(Constants.TableLayerName);
            MakeLayerCurrent(Constants.TableLayerName);
            InitLine();
            MakeLayerCurrent(Constants.DefaultZeroLayerName);
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

        public void InitPolyLine()
        {
            // Create a polyline with two segments (3 points)
            using (Polyline acPoly = new Polyline())
            {
                acPoly.AddVertexAt(0, new Point2d(2, 4), 0, 0, 0);
                acPoly.AddVertexAt(1, new Point2d(4, 2), 0, 0, 0);
                acPoly.AddVertexAt(2, new Point2d(6, 4), 0, 0, 0);

                // Add the new object to the block table record and the transaction
                _model.AppendEntity(acPoly);
                _transaction.AddNewlyCreatedDBObject(acPoly, true);
            }
        }
    }
}
