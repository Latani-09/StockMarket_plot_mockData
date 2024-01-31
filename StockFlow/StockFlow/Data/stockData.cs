namespace StockFlow.Data
{
    public class stockData
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Symbol { get; set; }
        public string Series { get; set; }
        public double PrevClose { get; set; }
        public double Open { get; set; }
        public double High { get; set; }

        public double Low { get; set; }
        public double Last { get; set; }
        public double Close { get; set; }
        public double VWAP { get; set; }
        public double Volume { get; set; } 
        public double Turnover { get; set; }
        public double Trades { get; set; }
        public double DeliverableVolume { get; set; }
        public double percDeliverble { get; set;}
    }
}
