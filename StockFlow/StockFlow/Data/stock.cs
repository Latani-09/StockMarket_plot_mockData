namespace StockFlow.Data
{
    internal class stock
    {
        public int timestamp { get; set; }
        public double gmtoffset  { get; set; }
        public string code { get; set; }
        public string Series { get; set; }
        public double previousClose { get; set; }
        public double Open { get; set; }
        public double High { get; set; }

        public double Low { get; set; }
        public double Last { get; set; }
        public double Close { get; set; }
        public double VWAP { get; set; }
        public double volume { get; set; }
        public double Turnover { get; set; }
        public double Trades { get; set; }
        public double change_p { get; set; }
        
    
    }
}
