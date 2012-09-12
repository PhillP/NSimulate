using System;
using NSimulate.Instruction;
using NSimulate;
using System.Collections.Generic;

namespace NSimulate.UnitTest
{
	public class TestActivity : Activity
	{
		public TestActivity (List<InstructionBase> instructions)
		{
			Instructions = instructions;
		}

		public List<InstructionBase> Instructions{
			get;
			private set;
		}

		public override System.Collections.Generic.IEnumerator<InstructionBase> Simulate ()
		{
			foreach(var instruction in Instructions){
				yield return instruction;
			}
		}
	}
}

