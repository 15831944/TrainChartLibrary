using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace TrainChartLibrary
{
    /// <summary>
    /// Создает элементы графика
    /// </summary>
    class ElementMaker
    {
        private ACadWorker _aCadWorker; // выполняет автокадовские команды
        private Dictionary<string, Point3d> lastTrainPointDictionary = new Dictionary<string, Point3d>(); // храним последнюю коорденату блока поезда

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

                _aCadWorker.CreateNewLayer(trainNumber, Constants.ElementLineWeight);
                _aCadWorker.MakeLayerCurrent(trainNumber);

                MakeElement(operationType, begin, _y, duration, trainNumber);
            }
            catch (IndexOutOfRangeException ignored) {} // для игнорирования лишних пробелов

            _aCadWorker.MakeLayerCurrent(Constants.DefaultZeroLayerName);
        }


        /// <summary>
        /// Создает отдельный элемент графика
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="duration"></param>
        /// /// <param name="trainNumber"></param>
        private void MakeElement(string operationType, int beginX, int beginY, int duration, string trainNumber)
        {
            switch (operationType)
            {
                case Constants.Waiting:
                {
                    MakeWaiting(beginX, beginY, duration);
                    break;
                }
                case Constants.Moving:
                {
                    MakeMoving(beginX, beginY, duration, trainNumber);
                    break;
                }
                case Constants.TO:
                {
                    MakeTO(beginX, beginY, duration);
                    break;
                }
                case Constants.BrakeTesting:
                {
                    MakeBrakeTesting(beginX, beginY, duration);
                    break;
                }
                case Constants.Coupling:
                {
                    MakeCoupling(beginX, beginY, duration);
                    break;
                }
                case Constants.UnCoupling:
                {
                    MakeUnCoupling(beginX, beginY, duration);
                    break;
                }
                case Constants.Loading:
                {
                    MakeLoading(beginX, beginY, duration);
                    break;
                }
                case Constants.UnLoading:
                {
                    MakeUnLoading(beginX, beginY, duration);
                    break;
                }
            }

            _aCadWorker.MakeBox(duration, 10, beginX, beginY); // выводит коробочку, если не знает, что нарисовать
        }

        private void MakeUnLoading(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakePolyline(beginX , beginY + Constants.ElementHeight, beginX + duration, beginY);
            _aCadWorker.MakeSolidRegion(beginX, beginY, beginX, beginY + Constants.ElementHeight, beginX + duration, beginY);
        }

        private void MakeLoading(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakePolyline(beginX, beginY, beginX + duration, beginY + Constants.ElementHeight);
            _aCadWorker.MakeSolidRegion(beginX, beginY, beginX + duration, beginY, beginX + duration, beginY + Constants.ElementHeight);
        }

        private void MakeUnCoupling(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakePolyline(beginX + 1, beginY + Constants.ElementHeight - 1, beginX + (double) duration/2, beginY + 2, beginX + duration - 1 , beginY + Constants.ElementHeight - 1);
        }

        private void MakeCoupling(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakePolyline(beginX + 1, beginY + 1, beginX + (double)duration / 2, beginY + Constants.ElementHeight - 2, beginX + duration - 1, beginY + 1);
        }

        private void MakeBrakeTesting(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakeMText(beginX + duration / 2, beginY + Constants.ElementHeight / 2, Constants.HeightOfInscriptions, duration, "BT", AttachmentPoint.MiddleCenter);
        }

        private void MakeTO(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakeMText(beginX + duration / 2, beginY + Constants.ElementHeight / 2, Constants.HeightOfInscriptions, duration, "TO", AttachmentPoint.MiddleCenter);
        }

        /// <summary>
        /// Реализовать соединение
        /// </summary>
        /// <param name="beginX"></param>
        /// <param name="beginY"></param>
        /// <param name="duration"></param>
        /// <param name="trainNumber"></param>
        private void MakeMoving(int beginX, int beginY, int duration, string trainNumber)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            _aCadWorker.MakePolyline(beginX, beginY, beginX + duration, beginY + Constants.ElementHeight);

            // для соединения
            MakeMovementLine(trainNumber, beginX, beginY);
        }

        private void MakeMovementLine(string trainNumber, int x, int y)
        {
            if (lastTrainPointDictionary.ContainsKey(trainNumber)) // если уже был блок движения 
            {
                Point3d point3D;
                lastTrainPointDictionary.TryGetValue(trainNumber, out point3D);
                _aCadWorker.MakePolyline((int) point3D.X, (int) point3D.Y, x, y); // рисуем линию
                lastTrainPointDictionary.Remove(trainNumber);
            }
            // добавляем новую точку
            lastTrainPointDictionary.Add(trainNumber, new Point3d(x, y, 0));
            //_aCadWorker.MakeMessage(String.Format("I've just add {0} - {1}", x, y));
        }

        private void MakeWaiting(int beginX, int beginY, int duration)
        {
            _aCadWorker.MakeBox(duration, Constants.ElementHeight, beginX, beginY);
            Point3dCollection point3DCollection = new Point3dCollection
            {
                new Point3d(beginX, beginY + Constants.ElementHeight/2, 0)
            };
            // первая точка

            // промежуточные точки
            int points = 10;
            if (duration > 200)
            {
                points = 20;
            }
            int x = duration / points;
            int y = Constants.ElementHeight / 3;
            for (int i = 1; i < points; i++)
            {
                
                point3DCollection.Add(new Point3d(beginX + i * x, beginY + ((i % 2) + 1)*y, 0));
            }

            point3DCollection.Add(new Point3d(beginX + duration, beginY + Constants.ElementHeight / 2, 0)); // последняя точка

            _aCadWorker.MakeSpline(point3DCollection);
        }

        public void PrintDict()
        {
            foreach (KeyValuePair<string, Point3d> kvp in lastTrainPointDictionary)
            {
                _aCadWorker.MakeMessage(String.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            }
        }
    }
}
