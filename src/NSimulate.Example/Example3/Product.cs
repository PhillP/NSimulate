using System;

namespace NSimulate.Example3
{
	/// <summary>
	/// A Product that can be delivered
	/// </summary>
	public class Product
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.Product"/> class.
		/// </summary>
		public Product ()
		{
		}

		/// <summary>
		/// Gets or sets the reorder count (inventory level at which a reorder is triggered).
		/// </summary>
		/// <value>
		/// The reorder count.
		/// </value>
		public int ReorderCount{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the reorder amount (quantity to reorder).
		/// </summary>
		/// <value>
		/// The reorder amount.
		/// </value>
		public int ReorderAmount{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the time required to reorder (in time periods)
		/// </summary>
		/// <value>
		/// The reorder time.
		/// </value>
		public int ReorderTime{
			get;
			set;
		}
	}
}

