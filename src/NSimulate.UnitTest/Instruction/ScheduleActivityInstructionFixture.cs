using NUnit.Framework;
using System;
using NSimulate.Instruction;
using System.Linq;
using NSimulate;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class ScheduleActivityInstructionFixture
	{
		[Test()]
		public void Complete_ActivitySpecified_ActivityScheduled()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){
			
				long waitTime = 10;
				var activity = new Activity();

				var instruction = new ScheduleActivityInstruction(activity, waitTime);

				long? nextTimePeriod;
				bool canComplete = instruction.CanComplete(context, out nextTimePeriod);

				Assert.IsTrue(canComplete);
				Assert.IsNull(nextTimePeriod);

				instruction.Complete(context);

				context.MoveToTimePeriod(0);

				var process = context.ActiveProcesses.FirstOrDefault(p=>p is ActivityHostProcess);
				var activityHost = process as ActivityHostProcess;

				Assert.IsNotNull(process);
				Assert.IsNotNull(activityHost);

				Assert.AreEqual(waitTime, activityHost.WaitTime);
				Assert.AreEqual(activity, activityHost.Activity);
			}
		}
	}
}

