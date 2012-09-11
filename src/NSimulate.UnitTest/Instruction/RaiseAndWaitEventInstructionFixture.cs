using NUnit.Framework;
using System;
using NSimulate.Instruction;
using System.Collections.Generic;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class WaitEventInstructionFixture
	{
		[Test()]
		public void CanComplete_BeforeAndAfterEvent_ReturnsTrueOnlyAfterEvent()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				var waitInstruction = new WaitEventInstruction<TestEvent>();
				var process = new InstructionListTestProcess(new List<InstructionBase>(){ waitInstruction});

				context.MoveToTimePeriod(0);
				process.SimulationState.InstructionEnumerator = process.Simulate();
				process.SimulationState.InstructionEnumerator.MoveNext();

				var testEvent = new TestEvent();
				var raiseInstruction = new RaiseEventInstruction<TestEvent>(testEvent);

				int? nextTimePeriodCheck;
				bool canComplete = waitInstruction.CanComplete(context, out nextTimePeriodCheck);

				Assert.IsFalse(canComplete);
				Assert.IsNull(nextTimePeriodCheck);

				raiseInstruction.Complete(context);

				canComplete = waitInstruction.CanComplete(context, out nextTimePeriodCheck);
				
				Assert.IsTrue(canComplete);
				Assert.IsNull(nextTimePeriodCheck);
			}
		}

		[Test()]
		public void CanComplete_BeforeAndAfterEventWithCondition_ReturnsTrueOnlyAfterEventMatchingCondition()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				var waitInstruction = new WaitEventInstruction<TestEvent>((e)=>e.Data > 0);
				var process = new InstructionListTestProcess(new List<InstructionBase>(){ waitInstruction});
				context.MoveToTimePeriod(0);
				process.SimulationState.InstructionEnumerator = process.Simulate();
				process.SimulationState.InstructionEnumerator.MoveNext();

				var testEvent1 = new TestEvent() { Data = 0 };
				var raiseInstruction1 = new RaiseEventInstruction<TestEvent>(testEvent1);

				var testEvent2 = new TestEvent() { Data = 1 };
				var raiseInstruction2 = new RaiseEventInstruction<TestEvent>(testEvent2);

				int? nextTimePeriodCheck;
				bool canComplete = waitInstruction.CanComplete(context, out nextTimePeriodCheck);
				
				Assert.IsFalse(canComplete);
				Assert.IsNull(nextTimePeriodCheck);

				raiseInstruction1.Complete(context);

				canComplete = waitInstruction.CanComplete(context, out nextTimePeriodCheck);
				
				Assert.IsFalse(canComplete);
				Assert.IsNull(nextTimePeriodCheck);

				raiseInstruction2.Complete(context);

				canComplete = waitInstruction.CanComplete(context, out nextTimePeriodCheck);
				
				Assert.IsTrue(canComplete);
				Assert.IsNull(nextTimePeriodCheck);
				Assert.AreEqual(1,waitInstruction.Events.Count);
				Assert.IsTrue(waitInstruction.Events.Contains(testEvent2));
			}
		}
	}
}

