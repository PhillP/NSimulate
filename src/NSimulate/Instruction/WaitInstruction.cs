using System;

namespace NSimulate.Instruction
{
	/// <summary>
	/// An instruction used to force a process to wait
	/// </summary>
	public class WaitInstruction : InstructionBase 
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.HoldInstruction"/> class.
		/// </summary>
		public WaitInstruction (int periods)
		{
			NumberOfPeriodsToWait = periods;
		}

		/// <summary>
		/// Gets or sets the number of periods to wait.
		/// </summary>
		/// <value>
		/// The number of periods to wait.
		/// </value>
		protected int NumberOfPeriodsToWait {
			get;
			set;
		}

		/// <summary>
		/// Determines whether this instruction can complete in the current time period
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can complete.
		/// </returns>
		/// <param name='context'>
		/// Context providing state information for the current simulation
		/// </param>
		/// <param name='skipFurtherChecksUntilTimePeriod'>
		/// Output parameter used to specify a time period at which this instruction should be checked again.  This should be left null if it is not possible to determine when this instruction can complete.
		/// </param>
		public override bool CanComplete(SimulationContext context, out int? skipFurtherChecksUntilTimePeriod){
			bool canComplete = false;

			int timePeriodToComplete = RaisedInTimePeriod + NumberOfPeriodsToWait;
			skipFurtherChecksUntilTimePeriod = timePeriodToComplete;
			if (context.TimePeriod >= timePeriodToComplete){
				canComplete = true;
			}

			return canComplete;
		}
	}
}

