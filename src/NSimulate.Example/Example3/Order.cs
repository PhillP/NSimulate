using System;

namespace NSimulate.Example3
{
	/// <summary>
	/// An Order to be delivered
	/// </summary>
	public class Order
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.Order"/> class.
		/// </summary>
		public Order ()
		{
		}

		/// <summary>
		/// Gets or sets the product to be delivered
		/// </summary>
		/// <value>
		/// The product.
		/// </value>
		public Product Product{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the quantity to be delivered
		/// </summary>
		/// <value>
		/// The quantity.
		/// </value>
		public int Quantity {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the delivery address.
		/// </summary>
		/// <value>
		/// The delivery address.
		/// </value>
		public Address DeliveryAddress{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the delivery time period.
		/// </summary>
		/// <value>
		/// The delivery time period.
		/// </value>
		public int? DeliveryTimePeriod {
			get;
			set;
		}
	}
}

