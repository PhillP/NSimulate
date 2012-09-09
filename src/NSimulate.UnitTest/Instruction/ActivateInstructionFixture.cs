using NUnit.Framework;
using System;
using NSimulate.Instruction;
using System.Collections.Generic;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class ActivateInstructionFixture
	{
		[Test()]
		public void TestCase ()
		{
		}

		[Test()]
		public void Complete_ContextPassed_ProcessActivated()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				Process testProcess = new Process();
				testProcess.SimulationState.IsActive = false;

				context.MoveToTimePeriod(0);

				Assert.IsFalse(context.ActiveProcesses.Contains(testProcess));

				var instruction = new ActivateInstruction(testProcess);

				int? nextTimePeriodCheck = null;
				bool canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsTrue(canComplete);
				instruction.Complete(context);

				Assert.IsTrue(testProcess.SimulationState.IsActive);
				Assert.IsTrue(context.ActiveProcesses.Contains(testProcess));
			}
		}
	}
}

