using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Printers
{
    public class PrinterFactory : IDisposable
    {

        readonly Type _iPrinterInterface = typeof(IPrinter);

        Dictionary<string, Func<IPrinter>> _iPrinterConstructorsById = null;

        public PrinterFactory()
        {
            BuildCache();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
        }

        public IPrinter NewPrinter(string id)
        {
            if (_iPrinterConstructorsById.TryGetValue(id, out var constructor))
            {
                return constructor.Invoke();
            }
            throw new NotFoundException($"Implementation of IPrinter with PrinterId = {id} was not found in Current AppDomain.");
        }


        bool ImplementsIPrinter(TypeInfo type) =>
            type.ImplementedInterfaces.Any(t => t == _iPrinterInterface);

        PrinterIdAttribute GetPrinterIdOrDefault(TypeInfo type) =>
            type.GetCustomAttributes(typeof(PrinterIdAttribute), false)
                .OfType<PrinterIdAttribute>()
                .FirstOrDefault();

        void BuildCache()
        {
            _iPrinterConstructorsById = new Dictionary<string, Func<IPrinter>>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                BuildCache(assembly);
            }
        }

        void BuildCache(Assembly assembly)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"PrinterFactory.BuildCache ({assembly.FullName})");
#endif
            foreach (var type in assembly.DefinedTypes)
            {
                if (ImplementsIPrinter(type))
                {
                    var attr = GetPrinterIdOrDefault(type);
                    if (attr != null)
                    {
                        var constructor = type.GetConstructor(Array.Empty<Type>());
                        _iPrinterConstructorsById.Add(attr.Id, () => (IPrinter)constructor.Invoke(Array.Empty<object>()));
                    }
                }
            }
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            BuildCache(args.LoadedAssembly);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
                }                
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PrinterIdAttribute : Attribute
    {

        public PrinterIdAttribute(string id)
        {
            this.Id = id;
        }

        public string Id { get; }

    }

}
