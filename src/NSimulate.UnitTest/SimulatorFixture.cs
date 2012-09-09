using NUnit.Framework;
using System;
using NSimulate;
using NSimulate.Instruction;
using System.Collections.Generic;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class SimulatorFixture
	{
		[Test()]
		public void Simulate_VariousProcesses_SimulateAsExpected ()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess: true)){
				var processor1Instructions = new List<InstructionBase>(){
					new WaitInstruction(5),
					new WaitInstruction(10)
				};

				var processor2Instructions = new List<InstructionBase>(){
					new WaitInstruction(2),
					new WaitInstruction(12),
				};

				var processor3Instructions = new List<InstructionBase>(){
					new WaitInstruction(9),
				};

				var processor1 = new InstructionListTestProcess(processor1Instructions);
				var processor2 = new InstructionListTestProcess(processor2Instructions);
				var processor3 = new InstructionListTestProcess(processor3Instructions);

				processor3.SimulationState.IsActive = false;
				processor2Instructions.Add(new ActivateInstruction(processor3));

				var simulator = new Simulator();

				simulator.Simulate();

				Assert.AreEqual(23, context.TimePeriod);

				Assert.AreEqual(5, processor1Instructions[0].CompletedAtTimePeriod);
				Assert.AreEqual(15, processor1Instructions[1].CompletedAtTimePeriod);

				Assert.AreEqual(2, processor2Instructions[0].CompletedAtTimePeriod);
				Assert.AreEqual(14, processor2Instructions[1].CompletedAtTimePeriod);

				Assert.AreEqual(23, processor3Instructions[0].CompletedAtTimePeriod);
			}
		}

		[Test()]
		public void Simulate_Terminated_SimulationEndsAtTerminatedPeriod ()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess: true)){
				var processor1Instructions = new List<InstructionBase>(){
					new WaitInstruction(5),
					new WaitInstruction(10)
				};

				var processor2Instructions = new List<InstructionBase>(){
					new WaitInstruction(2),
					new TerminateSimulationInstruction(),
					new WaitInstruction(9),
				};

				var processor1 = new InstructionListTestProcess(processor1Instructions);
				var processor2 = new InstructionListTestProcess(processor2Instructions);

				var simulator = new Simulator();

				simulator.Simulate();

				Assert.AreEqual(2, context.TimePeriod);

				Assert.IsNull(processor1Instructions[0].CompletedAtTimePeriod);
				Assert.IsNull(processor1Instructions[1].CompletedAtTimePeriod);
				Assert.AreEqual(2, processor2Instructions[0].CompletedAtTimePeriod);
				Assert.AreEqual(2, processor2Instructions[1].CompletedAtTimePeriod);
				Assert.IsNull(processor2Instructions[2].CompletedAtTimePeriod);
			}
		}

		[Test()]
		public void Simulate_ResourceContention_SimulationEndsAtExpectedPeriod ()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess: true)){

				new TestResource(1);

				var firstAllocation = new AllocateInstruction<TestResource>(1);
				var secondAllocation = new AllocateInstruction<TestResource>(1);
				var firstRelease = new ReleaseInstruction<TestResource>(firstAllocation);
				var secondRelease = new ReleaseInstruction<TestResource>(secondAllocation);

				var processor1Instructions = new List<InstructionBase>(){
					firstAllocation,
					new WaitInstruction(5),
					firstRelease,
					new WaitInstruction(10)
				};

				var processor2Instructions = new List<InstructionBase>(){
					secondAllocation,
					new WaitInstruction(2),
					secondRelease,
					new WaitInstruction(9),
				};

				var processor1 = new InstructionListTestProcess(processor1Instructions);
				var processor2 = new InstructionListTestProcess(processor2Instructions);

				var simulator = new Simulator();

				simulator.Simulate();

				// simulation time is extended due to resource contention
				Assert.AreEqual(16, context.TimePeriod);
			}
		}
	}
}

