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

        // константы для текстового файла
        public const char TrackNameSeparator = ':';
        public const char OperationsSeparator = ' ';
        public const char WordsInOperationSeparator = '-';

        // константы для элементов графика
        public const int HeightOfInscriptions = 8;                                 // высота надписей на элементах
        public const int ElementHeight = 10;                                       // высота элементов графика
        public const LineWeight ElementLineWeight = LineWeight.LineWeight040;      // толщина линии элементов графика
        public const string Waiting = "waiting";                                   // типы операций, доступные для программы
        public const string Moving = "moving";
        public const string TO = "to";
        public const string BrakeTesting = "brake";
        public const string Coupling = "coupling";
        public const string UnCoupling = "unCoupling";
        public const string Loading = "loading";
        public const string UnLoading = "unLoading";
    }
}
