using NUnit.Framework;
using System;
using NSimulate.Instruction;
using NSimulate;
using System.Collections.Generic;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class SimulationEndTriggerFixture
	{
		[Test()]
		public void Process_EndConditionSpecified_EndConditionMetAtExpectedTime ()
		{
			using(var context = new SimulationContext(isDefaultContextForProcess: true)){

				var waitInstruction1 = new WaitInstruction(2);
				var waitInstruction2 = new WaitInstruction(4);
				var waitInstruction3 = new WaitInstruction(4);

				var process = new InstructionListTestProcess(new List<InstructionBase>(){ waitInstruction1, waitInstruction2, waitInstruction3 });
				var endTrigger = new SimulationEndTrigger(()=>context.TimePeriod >= 5);

				var simulator = new Simulator();

				simulator.Simulate();

				Assert.AreEqual(6, context.TimePeriod);

				// the simulation should have ended at th expected time
				Assert.IsTrue(process.SimulationState.IsActive);
			}
		}
	}
}

