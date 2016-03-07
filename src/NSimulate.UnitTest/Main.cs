using System;
using NUnitLite;
using NUnit.Common;
using System.Reflection;

namespace NSimulate.Example
{
	/// <summary>
	/// Main class.
	/// </summary>
	class MainClass
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
        public static int Main(string[] args)
        {
            var autoRun = new AutoRun(typeof(MainClass).GetTypeInfo().Assembly);
            
            #if DNX451
                    autoRun.Execute(args);
            #else
                    autoRun.Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
            #endif
            
            return 0;
        }
    }
}