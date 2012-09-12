using System;
using NSimulate;

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
		public static void Main (string[] args)
		{
			bool exitOnNextLoop = false;
			bool skipLastCharacterExit = false;

			while(!exitOnNextLoop){
				exitOnNextLoop = true;
				Console.WriteLine("Choose an example:");
				Console.WriteLine("      1:  Workshop with unreliable machines");
				Console.WriteLine("      2:  Call center");
				Console.WriteLine("      3:  Order delivery with warehouse reorder");
				Console.WriteLine("      4:  Alarm Cklock");
				Console.WriteLine("      Q:  Quit");

				var keyInfo = Console.ReadKey(true);
				Console.WriteLine();

				switch(keyInfo.KeyChar.ToString().ToUpper()){
					case "1":
						Example1.Example.Run();
						break;
					case "2":
						Example2.Example.Run();
						break;
					case "3":
						Example3.Example.Run();
						break;
					case "4":
						Example4.Example.Run();
						break;
					case "Q":
					    skipLastCharacterExit = true;
						break;
					default:
						exitOnNextLoop = false;
						break;
				}
			}

			if(!skipLastCharacterExit){
				Console.WriteLine();
				Console.WriteLine("Press any key...");
				Console.ReadKey();
			}
		}
	}
}
