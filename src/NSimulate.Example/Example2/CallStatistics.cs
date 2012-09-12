using System;

namespace NSimulate.Example2
{
	/// <summary>
	/// Statistics captured for a call
	/// </summary>
	public class CallStatistics
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example2.CallStatistics"/> class.
		/// </summary>
		public CallStatistics ()
		{
		}

		/// <summary>
		/// Gets or sets the call start time period.
		/// </summary>
		/// <value>
		/// The call start time period.
		/// </value>
		public long? CallStartTimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call level1 time period.
		/// </summary>
		/// <value>
		/// The call level1 time period.
		/// </value>
		public long? CallLevel1TimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call level1 end time period.
		/// </summary>
		/// <value>
		/// The call level1 end time period.
		/// </value>
		public long? CallLevel1EndTimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call level2 time period.
		/// </summary>
		/// <value>
		/// The call level2 time period.
		/// </value>
		public long? CallLevel2TimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call level2 end time period.
		/// </summary>
		/// <value>
		/// The call level2 end time period.
		/// </value>
		public long? CallLevel2EndTimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call end time period.
		/// </summary>
		/// <value>
		/// The call end time period.
		/// </value>
		public long? CallEndTimePeriod{
			get;
			set;
		}

		/// <summary>
		/// Gets the time on hold before level1.
		/// </summary>
		/// <value>
		/// The time on hold before level1.
		/// </value>
		public long? TimeOnHoldBeforeLevel1{
			get{
				return CalculateTimeDifference(CallStartTimePeriod, CallLevel1TimePeriod);
			}
		}

		/// <summary>
		/// Gets the time on hold before level2.
		/// </summary>
		/// <value>
		/// The time on hold before level2.
		/// </value>
		public long? TimeOnHoldBeforeLevel2{
			get{
				return CalculateTimeDifference(CallLevel1EndTimePeriod, CallLevel2TimePeriod);
			}
		}

		/// <summary>
		/// Gets the time at level1.
		/// </summary>
		/// <value>
		/// The time at level1.
		/// </value>
		public long? TimeAtLevel1{
			get{
				return CalculateTimeDifference(CallLevel1TimePeriod, CallLevel1EndTimePeriod);
			}
		}

		/// <summary>
		/// Gets the time at level2.
		/// </summary>
		/// <value>
		/// The time at level2.
		/// </value>
		public long? TimeAtLevel2{
			get{
				return CalculateTimeDifference(CallLevel2TimePeriod, CallLevel2EndTimePeriod);
			}
		}

		/// <summary>
		/// Gets the total time on hold.
		/// </summary>
		/// <value>
		/// The total time on hold.
		/// </value>
		public long? TotalTimeOnHold{
			get{
				return (TimeOnHoldBeforeLevel1 ?? 0) + (TimeOnHoldBeforeLevel2 ?? 0);
			}
		}

		/// <summary>
		/// Gets the total time of call.
		/// </summary>
		/// <value>
		/// The total time of call.
		/// </value>
		public long? TotalTimeOfCall{
			get{
				return CalculateTimeDifference(CallStartTimePeriod, CallEndTimePeriod);
			}
		}

		/// <summary>
		/// Calculates the time difference between two values
		/// </summary>
		/// <returns>
		/// The time difference.
		/// </returns>
		/// <param name='start'>
		/// Start.
		/// </param>
		/// <param name='end'>
		/// End.
		/// </param>
		public long? CalculateTimeDifference(long? start, long? end){
			long? difference = 0;

			if (start != null && end != null)
			{
				difference = end - start;
			}

			return difference;
		}
	}
}
