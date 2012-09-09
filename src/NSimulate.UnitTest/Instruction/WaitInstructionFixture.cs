using NUnit.Framework;
using System;
using NSimulate.Instruction;

namespace NSimulate.UnitTest
{
	/// <summary>
	/// Test fixture for the WaitInstruction
	/// </summary>
	[TestFixture()]
	public class WaitInstructionFixture
	{
		[Test()]
		public void CanComplete_Various_ReturnsFalseUntilWaitTimeElapsed()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				int waitPeriods = 10;
				var instruction = new WaitInstruction(waitPeriods);

				bool canComplete = false;
				int? nextTimePeriodCheck;

				for(int i=1;i<waitPeriods;i++){
					context.MoveToTimePeriod(i);

					canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);
					Assert.IsFalse(canComplete);
					Assert.AreEqual(waitPeriods, nextTimePeriodCheck);
				}

				context.MoveToTimePeriod(waitPeriods);
				canComplete = instruction.CanComplete(context, out nextTimePeriodCheck);
				Assert.IsTrue(canComplete);
			}
		}
	}
}

