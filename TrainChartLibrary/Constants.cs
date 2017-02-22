using Autodesk.AutoCAD.DatabaseServices;

namespace TrainChartLibrary
{
    class Constants
    {
        public const string TableLayerName = "TableLayer";                          // названия слоя сетки графики
        public const string DefaultZeroLayerName = "0";                             // название дефолтного слоя
        public const int HeightOfRow = 20;                                          // высота строки графика
        public const int MinutesInDay = 1440;                                       // ширина графика
        public const LineWeight TableFatLineWeight = LineWeight.LineWeight040;      // толщина жирной линии сетки графика 
        public const string DashSpaceLineType = "ACAD_ISO03W100";                   // тип линии для получасовых линий
    }
}
