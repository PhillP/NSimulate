using NUnit.Framework;
using System;
using NSimulate;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class ProcessFixture
	{
		[Test()]
		public void Constructor_SimulationContextExists_ProcessRegistered()
		{
			using(var context = new SimulationContext(isDefaultContextForProcess: true)){
				var process = new Process();

				Assert.IsNotNull(process.SimulationState);
				Assert.IsTrue(process.SimulationState.IsActive);

				var registeredProcesses = context.GetByType<Process>();
				Assert.IsTrue(registeredProcesses.Contains(process));
			}
		}

		[Test()]
		public void Simulate_EnumerableReturned()
		{
			var process = new Process();

			var enumerator = process.Simulate();

			Assert.IsNotNull(enumerator);
			Assert.IsFalse(enumerator.MoveNext());
		}
	}
}

