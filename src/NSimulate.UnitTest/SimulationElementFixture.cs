using NUnit.Framework;
using System;
using NSimulate;
using System.Linq;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class SimulationElementFixture
	{
		#region Test Types

		private class TestElement : SimulationElement
		{
		}

		#endregion

		[Test()]
		public void Constructor_SimulationContextExists_ResourceRegistered()
		{
			using(var context = new SimulationContext(isDefaultContextForProcess: true)){
				var element = new TestElement();

				var registeredElements = context.GetByType<TestElement>();
				Assert.IsTrue(registeredElements.Contains(element));
			}
		}
	}

}

