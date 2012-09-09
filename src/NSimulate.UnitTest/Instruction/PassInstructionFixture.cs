using NUnit.Framework;
using System;
using NSimulate;
using NSimulate.Instruction;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class PassInstructionFixture
	{
		[Test()]
		public void CanComplete_ContextPassed_ReturnsFalseUntilTimePeriodChanged()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				context.MoveToTimePeriod(0);
				var instruction = new PassInstruction();

				int? nextTimePeriodCheck = null;
				bool canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsFalse(canComplete);
				Assert.IsNull(nextTimePeriodCheck);

				context.MoveToTimePeriod(1);
				canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsTrue(canComplete);
				Assert.IsNull(nextTimePeriodCheck);
			}
		}
	}
}

