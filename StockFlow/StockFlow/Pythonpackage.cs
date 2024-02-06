
using Python.Runtime;
using System.Threading.Tasks;
using Python.Included;


namespace StockFlow
{
    public class Pythonpackage
    {
        public static async Task Main()
        {
            await Installer.SetupPython();
            await Installer.TryInstallPip();
           await  Installer.PipInstallModule("yfinance");
            PythonEngine.Initialize();
            dynamic sys = Py.Import("sys");
            Console.WriteLine("Python version: " + sys.version);
            dynamic yfinance = Py.Import("pyfinance");
            Console.WriteLine("Spacy version: " + yfinance.__version__);
            var msft = yfinance.Ticker("MSFT");
            Console.WriteLine(msft.info);

            
        }
    }
}
