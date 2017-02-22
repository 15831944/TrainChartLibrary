
namespace TrainChartLibrary
{
    class LineGenerator : IGenerator
    {
        private string _line;
        private ACadWorker _aCadWorker;

        public LineGenerator(ACadWorker aCadWorker, string line)
        {
            _aCadWorker = aCadWorker;
            _line = line;
        }

        public void Generate()
        {
            
        }
    }
}
