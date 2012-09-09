using System;

namespace NSimulate.Example3
{
	/// <summary>
	/// An Address for order delivry
	/// </summary>
	public class Address
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.Address"/> class.
		/// </summary>
		public Address ()
		{
			var rand = new Random();

			// generate some coordinates
			X = rand.Next(100);
			Y = rand.Next(100);
		}

		/// <summary>
		/// Gets or sets the x coordinate of the address
		/// </summary>
		/// <value>
		/// The x.
		/// </value>
		public int X{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the y coordinate of the address
		/// </summary>
		/// <value>
		/// The y.
		/// </value>
		public int Y{
			get;
			set;
		}
	}
}
