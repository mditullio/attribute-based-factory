using Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSubclassFactoryWithAttributes
{
    [PrinterId("Epson")]
    public class EpsonPrinter : IPrinter
    {

        public string Print()
        {
            return ">>>> Printing with Epson <<<<<";
        }

    }

    [PrinterId("HP")]
    public class HpPrinter : IPrinter
    {

        public string Print()
        {
            return "<<<< Printing with HP >>>>";
        }

    }

    public class PrinterWithoutId : IPrinter
    {

        public string Print()
        {
            return "Will never be called";
        }

    }
}
