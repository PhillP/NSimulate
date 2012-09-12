using NUnit.Framework;
using System;
using NSimulate;
using NSimulate.Instruction;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class TerminateSimulationInstructionFixture
	{
		[Test()]
		public void Complete_ContextPassed_ContextFlaggedForSimulationTerminate()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				Assert.IsFalse(context.IsSimulationTerminating);
				var instruction = new TerminateSimulationInstruction();

				long? nextTimePeriodCheck = null;
				bool canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsTrue(canComplete);
				instruction.Complete(context);

				Assert.IsTrue(context.IsSimulationTerminating);
			}
		}
	}
}

