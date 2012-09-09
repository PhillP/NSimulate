using NUnit.Framework;
using System;
using NSimulate.Instruction;
using NSimulate;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class CompositeInstructionFixture
	{
		#region Private Types

		public class TestInstruction : InstructionBase
		{
			public bool CanCompleteResult {
				get;
				set;
			}

			public int? CanCompleteNextTimePeriodResult {
				get;
				set;
			}

			public bool HasCanCompleteBeenCalled{
				get;
				set;
			}

			public bool HasCompleteBeenCalled{
				get;
				set;
			}

			public override bool CanComplete (SimulationContext context, out int? skipFurtherChecksUntilTimePeriod)
			{
				HasCanCompleteBeenCalled = true;

				skipFurtherChecksUntilTimePeriod = CanCompleteNextTimePeriodResult;
				return CanCompleteResult;
			}

			public override void Complete (SimulationContext context)
			{
				HasCompleteBeenCalled = true;
			}
		}

		#endregion

		[Test()]
		public void CanComplete_ContainedInstructionsCalled_CanCompleteOnlyWhenAllContainedInstructionsCan()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){
				// create a composite instruction with test instructions
				var testInstructions = new List<TestInstruction>();
				for(int i = 1; i<=10; i++){
					testInstructions.Add(new TestInstruction() { CanCompleteResult = true, CanCompleteNextTimePeriodResult = i });
				}
				var compositeInstruction = new CompositeInstruction(testInstructions.Cast<InstructionBase>().ToList());

				// when all contained instructions can complete, the composite instruction cancomplete call returns true
				int? nextTimePeriodCheck = null;
				bool canComplete = compositeInstruction.CanComplete(context, out nextTimePeriodCheck);
				Assert.IsTrue(canComplete);
				Assert.IsNull(nextTimePeriodCheck);

				foreach(var testInstruction in testInstructions){
					Assert.IsTrue(testInstruction.HasCanCompleteBeenCalled);
				}

				// when some of the contained instructions can not complete, the composite instruction can complete call returns false
				for(int i = 0; i<=3; i++){
					testInstructions[i].CanCompleteResult = false;
				}
				canComplete = compositeInstruction.CanComplete(context, out nextTimePeriodCheck);
				Assert.IsFalse(canComplete);
				// the next time period check is the lowest of any contained instruction next period values
				Assert.AreEqual(1, nextTimePeriodCheck);

				// the next time period check value is returned as null if any contained instruction returns null
				testInstructions[0].CanCompleteNextTimePeriodResult = null;
				canComplete = compositeInstruction.CanComplete(context, out nextTimePeriodCheck);
				Assert.IsFalse(canComplete);
				Assert.IsNull(nextTimePeriodCheck);
			}
		}

		[Test()]
		public void Complete_ContainedInstructionsCalled_AllContainedInstructionsCompleted()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){
				// create a composite instruction with test instructions
				var testInstructions = new List<TestInstruction>();
				for(int i = 1; i<=10; i++){
					testInstructions.Add(new TestInstruction() { CanCompleteResult = true, CanCompleteNextTimePeriodResult = i });
				}
				var compositeInstruction = new CompositeInstruction(testInstructions.Cast<InstructionBase>().ToList());

				compositeInstruction.Complete(context);
				foreach(var testInstruction in testInstructions){
					Assert.IsTrue(testInstruction.HasCompleteBeenCalled);
				}
			}
		}
	}
}

