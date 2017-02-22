
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

            // создаем саму таблицу
            _aCadWorker.MakeBox(Constants.MinutesInDay, _rowsNumber * Constants.HeightOfRow, Constants.TableFatLineWeight);
            MakeGrid(_rowsNumber);
           

            // делаем текущем дефолтный слой
            _aCadWorker.MakeLayerCurrent(Constants.DefaultZeroLayerName);
        }

        private void MakeGrid(int numberOfRows)
        {
            // высота линий сетки
            int heightOfLine = numberOfRows * Constants.HeightOfRow;
            // часовые линии
            for (int i = 60; i < 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, 0, i, heightOfLine, Constants.TableFatLineWeight);
            }
            // получасовые линии
            _aCadWorker.LoadLineType(Constants.DashSpaceLineType);
            for (int i = 30; i < 1440; i = i + 60)
            {
                _aCadWorker.MakePolyline(i, 0, i, heightOfLine, Constants.DashSpaceLineType);
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
