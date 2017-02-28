
using System;

namespace TrainChartLibrary
{
    class TableGenerator : IGenerator
    {
        private readonly int _rowsNumber;
        private readonly ACadWorker _aCadWorker;

        public TableGenerator(ACadWorker aCadWorker, int rowsNumber)
        {
            _aCadWorker = aCadWorker;
            _rowsNumber = rowsNumber;
        }

        /// <summary>
        /// Основная функция - создает таблицу для графика
        /// </summary>
        public void Generate()
        {
            // создаем и делаем текущем слой для таблицы
            _aCadWorker.CreateNewLayer(Constants.TableLayerName);
            _aCadWorker.MakeLayerCurrent(Constants.TableLayerName);

            // сетка графика для рисовния
            MakeGrid(_rowsNumber);
            // таблица вокруг графика
            MakeFrame(_rowsNumber);
           

            // делаем текущем дефолтный слой
            _aCadWorker.MakeLayerCurrent(Constants.DefaultZeroLayerName);
        }

        /// <summary>
        /// Делает рамочку вокруг сетки
        /// </summary>
        /// <param name="numberOfRows"></param>
        private void MakeFrame(int numberOfRows)
        {
            // высота шапки таблицы
            int hatHeight = numberOfRows * Constants.HeightOfRow;

            MakeTopPartOfFrame(hatHeight);
            MakeLeftPartOfFrame(hatHeight);
        }

        /// <summary>
        /// Делает левую часть сетки
        /// </summary>
        /// <param name="hatHeight"></param>
        private void MakeLeftPartOfFrame(int hatHeight)
        {
            // вертикальная линия 1 (самая левая)
            int x = -(Constants.LengthOfNumberTrack + Constants.LengthOfTrackName);
            int y = hatHeight + Constants.HeightOfHat;
            _aCadWorker.MakePolyline(x, 0, x, y, Constants.TableFatLineWeight);
            // вертикальная линия 2
            _aCadWorker.MakePolyline(-Constants.LengthOfTrackName, 0, -Constants.LengthOfTrackName, y, Constants.TableFatLineWeight);
            // левые горизонтальные линии (тонкие)
            for (int i = Constants.HeightOfRow; i < hatHeight; i = i + Constants.HeightOfRow)
            {
                _aCadWorker.MakePolyline(x, i, 0, i);
            }
            // 3 толстые горизонтальные линии
            _aCadWorker.MakePolyline(x, 0, 0, 0, Constants.TableFatLineWeight);
            _aCadWorker.MakePolyline(x, y, 0, y, Constants.TableFatLineWeight);
            _aCadWorker.MakePolyline(x, hatHeight, 0, hatHeight, Constants.TableFatLineWeight);

            // нумерация путей
            MakeLeftGigits(_rowsNumber);
        }

        /// <summary>
        /// Делает нумерацию путей и №
        /// </summary>
        /// <param name="rowsNumber"></param>
        private void MakeLeftGigits(int rowsNumber)
        {
            int x = -(Constants.LengthOfNumberTrack + Constants.LengthOfTrackName);
            int y = 0;
            for (int i = rowsNumber; i > 0; i--) // это нумерация от конца
            {
                _aCadWorker.MakeMText(x + 7, y + Constants.HeightOfNumbers + 5, Constants.HeightOfNumbers, i.ToString());
                y += Constants.HeightOfRow;
            }
            _aCadWorker.MakeMText(x + 7, y + 40, 8, 15, "№ п/п");
        }

        /// <summary>
        /// Делает правую часть сетки
        /// </summary>
        /// <param name="hatHeight"></param>
        private void MakeTopPartOfFrame(int hatHeight)
        {
            // вериткальные часовые линии шапки
            int y = hatHeight + Constants.HeightOfHat;
            for (int i = 0; i <= 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, hatHeight, i, y, Constants.TableFatLineWeight);
            }
            // горизонтальная линия
            _aCadWorker.MakePolyline(0, y, Constants.MinutesInDay, y, Constants.TableFatLineWeight);
            // часовые цифорки
            MakeHourDigits(hatHeight);
        }

        /// <summary>
        /// Делает часовые цифры
        /// </summary>
        /// <param name="hatHeight"></param>
        private void MakeHourDigits(int hatHeight)
        {
            int nextNumber = 18;
            int dist = 0;
            while (true)
            {
                // записываем очередную цифорку
                _aCadWorker.MakeMText(Constants.Indent + dist, hatHeight + Constants.HeightOfNumbers + Constants.Indent, Constants.HeightOfNumbers, nextNumber.ToString());
                dist += 60;
                nextNumber++;
                if (nextNumber == 24)
                {
                    nextNumber = 0;
                }
                if (nextNumber == 18)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Делает сетку графика
        /// </summary>
        /// <param name="numberOfRows"></param>
        private void MakeGrid(int numberOfRows)
        {
            _aCadWorker.MakeBox(Constants.MinutesInDay, _rowsNumber * Constants.HeightOfRow, Constants.TableFatLineWeight);
            // высота линий сетки
            int heightOfLine = numberOfRows * Constants.HeightOfRow;
            // часовые линии
            for (int i = 60; i < 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, 0, i, heightOfLine, Constants.TableFatLineWeight);
            }
            // получасовые линии
            _aCadWorker.LoadLineType(Constants.DashSpaceLineType); // загружаем тип для получасовых линий
            for (int i = 30; i < 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, 0, i, heightOfLine, Constants.DashSpaceLineType, 0.5);
                ///////////////////// теперь измени масштаб линии и дальше
            }
            // разделители строк
            for (int i = Constants.HeightOfRow; i < heightOfLine; i = i + Constants.HeightOfRow)
            {
                _aCadWorker.MakePolyline(0, i, Constants.MinutesInDay, i);
            }
        }
    }
}
