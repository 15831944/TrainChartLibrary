using Autodesk.AutoCAD.DatabaseServices;

namespace TrainChartLibrary
{
    class Constants
    {
        // константы для сетки графика
        public const string TableLayerName = "TableLayer";                          // названия слоя сетки графика
        public const string DefaultZeroLayerName = "0";                             // название дефолтного слоя
        public const int HeightOfRow = 20;                                          // высота строки графика
        public const int MinutesInDay = 1440;                                       // ширина графика
        public const LineWeight TableFatLineWeight = LineWeight.LineWeight040;      // толщина жирной линии сетки графика 
        public const string DashSpaceLineType = "ACAD_ISO03W100";                   // тип линии для получасовых линий

        // константы для рамки графика
        public const int HeightOfHat = 55;
        public const int LengthOfTrackName = 200;
        public const int LengthOfNumberTrack = 25;
        public const int HeightOfNumbers = 8;
        public const int Indent = 2;
    }
}
