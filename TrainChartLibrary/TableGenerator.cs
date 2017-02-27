
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

        private void MakeFrame(int numberOfRows)
        {
            // Верхняя часть шапки
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // высота шапки таблицы
            int hatHeight = numberOfRows * Constants.HeightOfRow;
            // вериткальные часовые линии шапки
            int y = hatHeight + Constants.HeightOfHat;
            for (int i = 0; i <= 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, hatHeight, i, y, Constants.TableFatLineWeight);
            }
            // горизонтальная линия
            _aCadWorker.MakePolyline(0, y, Constants.MinutesInDay, y, Constants.TableFatLineWeight);
            // часовые цифорки

            _aCadWorker.MakeMText(Constants.Indent, hatHeight + Constants.HeightOfNumbers + Constants.Indent, Constants.HeightOfNumbers, "17");

            // Левая часть шапки
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // вертикальная линия 1
            int x = -(Constants.LengthOfNumberTrack + Constants.LengthOfTrackName);
            _aCadWorker.MakePolyline(x, 0, x, y, Constants.TableFatLineWeight);
            // вертикальная линия 2
            _aCadWorker.MakePolyline(-Constants.LengthOfTrackName, 0, -Constants.LengthOfTrackName, y, Constants.TableFatLineWeight);
            // левые горизонтальные линии (тонкие)
            for (int i = Constants.HeightOfRow; i <= hatHeight; i = i + Constants.HeightOfRow)
            {
                _aCadWorker.MakePolyline(x, i, 0, i);
            }
            // 2 толстые горизонтальные линии
            _aCadWorker.MakePolyline(x, 0, 0, 0, Constants.TableFatLineWeight);
            _aCadWorker.MakePolyline(x, y, 0, y, Constants.TableFatLineWeight);
        }

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
