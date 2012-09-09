using System;
using NSimulate;
using NSimulate.Instruction;
using System.Collections.Generic;

namespace NSimulate.Example2
{
	/// <summary>
	/// Class used to represent the process of handling a customer phone call
	/// </summary>
	public class Call : Process
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example.Call"/> class.
		/// </summary>
		/// <param name='characteristics'>
		/// Characteristics that drive behaviour for the call
		/// </param>
		public Call (CallCharacteristics characteristics)
		{
			Characteristics = characteristics;
			Statistics = new CallStatistics();
		}

		/// <summary>
		/// Gets or sets the characteristics that drive behaviour for this call.
		/// </summary>
		/// <value>
		/// The characteristics.
		/// </value>
		public CallCharacteristics Characteristics {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the statistics instance for this call
		/// </summary>
		/// <value>
		/// The statistics recorded for this call.
		/// </value>
		public CallStatistics Statistics {
			get;
			set;
		}

		/// <summary>
		///  Simulate this phone call 
		/// </summary>
		public override IEnumerator<InstructionBase> Simulate()
		{
			if (Characteristics.TimeBeforeCallStarts > 0){
				// wait before the call starts
				yield return new WaitInstruction(Characteristics.TimeBeforeCallStarts);
			}

			// record the time period in which the call is started
			Statistics.CallStartTimePeriod = Context.TimePeriod;

			// the call is answered by a call center worker
			// the time required to obtain this resource is the time the customer is on hold
			var level1StaffMemberAllocateInstruction = new AllocateInstruction<Level1CallCenterStaffMember>(1);
			yield return level1StaffMemberAllocateInstruction;

			Statistics.CallLevel1TimePeriod = Context.TimePeriod;

			// keep hold of the resource for the call duration... this represents the time the customer spends with the call center worker at level 1
			yield return new WaitInstruction(Characteristics.CallDurationAtLevel1);

			// Level 1 portion of the call is ended
			Statistics.CallLevel1EndTimePeriod = Context.TimePeriod;

			// release the first call resource
			yield return new ReleaseInstruction<Level1CallCenterStaffMember>(level1StaffMemberAllocateInstruction);

			if(Characteristics.CallIsEscalatedToLevel2){
				// if the call is elevated to level 2
				// request a level 2 call center responder
				// the time taken to obtain this resource is the time the customer spends on hold in the second queue
				var level2StaffMemberAllocateInstruction = new AllocateInstruction<Level2CallCenterStaffMember>(1);
				yield return level2StaffMemberAllocateInstruction;

				Statistics.CallLevel2TimePeriod = Context.TimePeriod;

				// hold the resource for the call duration at level 2...
				yield return new WaitInstruction(Characteristics.CallDurationAtLevel2);

				// Level 2 portion of the call is ended
				Statistics.CallLevel2EndTimePeriod = Context.TimePeriod;

				// release the second call resource
				yield return new ReleaseInstruction<Level2CallCenterStaffMember>(level2StaffMemberAllocateInstruction);
			}

			// the call is complete
			Statistics.CallEndTimePeriod = Context.TimePeriod;
		}
	}
}

