using Printers;
using System;
using Should;

namespace TestSubclassFactoryWithAttributes
{
    class Program
    {
        static void Main(string[] args)
        {
            PrinterFactory printerFactory = new PrinterFactory();
            IPrinter printer1 = printerFactory.NewPrinter("Epson");
            IPrinter printer2 = printerFactory.NewPrinter("HP");

            printer1.ShouldBeType<EpsonPrinter>();
            printer2.ShouldBeType<HpPrinter>();
            new Action(() => printerFactory.NewPrinter("Other")).ShouldThrow<NotFoundException>();

            Console.WriteLine("Test done");
            Console.ReadKey(false);
        }
    }
}
