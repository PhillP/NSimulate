using System;

namespace NSimulate.UnitTest
{
	public class TestResource : Resource
	{
		public TestResource (int capacity)
			: base(capacity)
		{
		}

		public string Code
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}
	}
}

