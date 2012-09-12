using NUnit.Framework;
using System;
using NSimulate.Instruction;
using NSimulate;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class AllocateInstructionFixture
	{
		[Test()]
		public void Complete_Multiple_ReturnsTrueOnlyWhenEnoughResources()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				int resouceCapacity = 5;
				var testResourceSet1 = new TestResource(resouceCapacity) { Code = "First", Priority = 10 };
				context.Register<TestResource>(testResourceSet1);

				// try to allocate one more than the total resources
				var allocateInstruction = new AllocateInstruction<TestResource>(resouceCapacity + 1);

				long? nextTimePeriodCheck;
				bool canComplete = allocateInstruction.CanComplete(context, out nextTimePeriodCheck);

				// allocation not possilbe
				Assert.IsNull(nextTimePeriodCheck);
				Assert.IsFalse(canComplete);

				// try to allocate the max available
				allocateInstruction = new AllocateInstruction<TestResource>(resouceCapacity);
				canComplete = allocateInstruction.CanComplete(context, out nextTimePeriodCheck);

				// allocation is possible
				Assert.IsNull(nextTimePeriodCheck);
				Assert.IsTrue(canComplete);

				allocateInstruction.Complete(context);

				Assert.AreEqual(resouceCapacity, testResourceSet1.Allocated);
				Assert.AreEqual(testResourceSet1.Allocated, allocateInstruction
				   .Allocations
				   .First(al=>al.Key == testResourceSet1)
				   .Value);
			}
		}

		[Test()]
		public void Complete_WithPriority_ResourcesAllocatedInPriorityOrder()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				int resouceCapacity = 10;
				var testResourceSet1 = new TestResource(resouceCapacity) { Code = "First", Priority = 10 };
				var testResourceSet2 = new TestResource(resouceCapacity) { Code = "Second", Priority = 20 };
				var testResourceSet3 = new TestResource(resouceCapacity) { Code = "Third", Priority = 5 };

				context.Register<TestResource>(testResourceSet1);
				context.Register<TestResource>(testResourceSet2);
				context.Register<TestResource>(testResourceSet3);

				// try to allocate 
				// request a number that will use 1 and 1/2 of the resource sets
				var allocateInstruction = new AllocateInstruction<TestResource>((int)(resouceCapacity * 1.5), resourcePriorityFunction: (r)=>r.Priority);

				long? nextTimePeriodCheck;
				bool canComplete = allocateInstruction.CanComplete(context, out nextTimePeriodCheck);

				// allocation should be possible
				Assert.IsNull(nextTimePeriodCheck);
				Assert.IsTrue(canComplete);

				allocateInstruction.Complete(context);

				Assert.AreEqual(resouceCapacity, testResourceSet3.Allocated);
				Assert.AreEqual(resouceCapacity / 2, testResourceSet1.Allocated);
				Assert.AreEqual(0, testResourceSet2.Allocated);

				Assert.AreEqual(testResourceSet1.Allocated, allocateInstruction.Allocations
				   .First(al=>al.Key == testResourceSet1)
				   .Value);
				Assert.AreEqual(testResourceSet3.Allocated, allocateInstruction.Allocations
				   .First(al=>al.Key == testResourceSet3)
				   .Value);
			}
		}

		[Test()]
		public void Complete_WithFilter_ResourcesAllocatedInPriorityOrder()
		{
			using (var context = new SimulationContext(isDefaultContextForProcess:true)){

				int resouceCapacity = 10;
				var testResourceSet1 = new TestResource(resouceCapacity) { Code = "First", Priority = 10 };
				var testResourceSet2 = new TestResource(resouceCapacity) { Code = "Second", Priority = 20 };
				var testResourceSet3 = new TestResource(resouceCapacity) { Code = "Third", Priority = 5 };

				context.Register<TestResource>(testResourceSet1);
				context.Register<TestResource>(testResourceSet2);
				context.Register<TestResource>(testResourceSet3);

				// try to allocate 
				// request a number that will use 1 and 1/2 of the resource sets
				var allocateInstruction = new AllocateInstruction<TestResource>((int)(resouceCapacity * 1.5), 
					resourcePriorityFunction: (r)=>r.Priority, 
				    resourceMatchFunction: (r)=>r.Code != "First");

				long? nextTimePeriodCheck;
				bool canComplete = allocateInstruction.CanComplete(context, out nextTimePeriodCheck);

				// allocation should be possible
				Assert.IsNull(nextTimePeriodCheck);
				Assert.IsTrue(canComplete);

				allocateInstruction.Complete(context);

				Assert.AreEqual(resouceCapacity, testResourceSet3.Allocated);
				Assert.AreEqual(resouceCapacity / 2, testResourceSet2.Allocated);
				Assert.AreEqual(0, testResourceSet1.Allocated);

				Assert.AreEqual(testResourceSet2.Allocated, allocateInstruction.Allocations
				   .First(al=>al.Key == testResourceSet2)
				   .Value);
				Assert.AreEqual(testResourceSet3.Allocated, allocateInstruction.Allocations
				   .First(al=>al.Key == testResourceSet3)
				   .Value);
			}
		}
	}
}