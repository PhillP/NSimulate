using System;

namespace NSimulate.Example2
{
	/// <summary>
	/// Characteristics describing attributes of a call
	/// </summary>
	public class CallCharacteristics
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example2.CallCharacteristics"/> class.
		/// </summary>
		public CallCharacteristics ()
		{
		}

		/// <summary>
		/// Gets or sets the time (in terms of number of time periods) before call starts.
		/// </summary>
		/// <value>
		/// The time before call starts.
		/// </value>
		public int TimeBeforeCallStarts{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call duration (in terms of time periods) at level1.
		/// </summary>
		/// <value>
		/// The call duration at level1.
		/// </value>
		public int CallDurationAtLevel1{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the call duration (in terms of time periods) at level2.
		/// </summary>
		/// <value>
		/// The call duration at level2.
		/// </value>
		public int CallDurationAtLevel2{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="NSimulate.Example2.CallCharacteristics"/> call is escalated to level2.
		/// </summary>
		/// <value>
		/// <c>true</c> if call is escalated to level2; otherwise, <c>false</c>.
		/// </value>
		public bool CallIsEscalatedToLevel2{
			get;
			set;
		}
	}
}
