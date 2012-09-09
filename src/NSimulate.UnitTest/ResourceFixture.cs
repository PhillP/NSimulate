using NUnit.Framework;
using System;
using NSimulate;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate.UnitTest
{
	[TestFixture()]
	public class ResourceFixture
	{
		[Test()]
		public void Constructor_SimulationContextExists_ResourceRegistered()
		{
			using(var context = new SimulationContext(isDefaultContextForProcess: true)){
				int capacity = 10;
				var resource = new Resource(capacity);

				Assert.AreEqual(0, resource.Allocated);
				Assert.AreEqual(capacity, resource.Capacity);

				var registeredResources = context.GetByType<Resource>();
				Assert.IsTrue(registeredResources.Contains(resource));
			}
		}
	}
}

