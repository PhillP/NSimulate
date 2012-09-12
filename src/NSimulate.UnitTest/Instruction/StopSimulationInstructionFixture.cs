using NUnit.Framework;
using System;
using NSimulate;
using NSimulate.Instruction;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class StopSimulationInstructionFixture
	{
		[Test()]
		public void Complete_ContextPassed_ContextFlaggedForSimulationStop()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				Assert.IsFalse(context.IsSimulationStopping);
				var instruction = new StopSimulationInstruction();

				long? nextTimePeriodCheck = null;
				bool canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsTrue(canComplete);
				instruction.Complete(context);

				Assert.IsTrue(context.IsSimulationStopping);
			}
		}
	}
}

