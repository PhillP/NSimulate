using System;
using System.Collections.Generic;
using NSimulate;

namespace NSimulate.Example3
{
	/// <summary>
	/// A warehouse inventory used to record quantities by product
	/// </summary>
	public class WarehouseInventory : SimulationElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.WarehouseInventory"/> class.
		/// </summary>
		public WarehouseInventory ()
		{
			QuantityByProduct = new Dictionary<Product, int>();
		}

		/// <summary>
		/// Gets or sets the quantity by product.
		/// </summary>
		/// <value>
		/// The quantity by product.
		/// </value>
		protected Dictionary<Product, int> QuantityByProduct {
			get;
			set;
		}

		/// <summary>
		/// Add the specified quantity of the specified product to this inventory
		/// </summary>
		/// <param name='product'>
		/// Product.
		/// </param>
		/// <param name='quantity'>
		/// Quantity.
		/// </param>
		public void Add(Product product, int quantity){
			QuantityByProduct[product] = CheckQuantity(product) + quantity;
		}

		/// <summary>
		/// Remove the specified quantity of the specified product
		/// </summary>
		/// <param name='product'>
		/// Product.
		/// </param>
		/// <param name='quantity'>
		/// Quantity.
		/// </param>
		public void Remove(Product product, int quantity){
			QuantityByProduct[product] = CheckQuantity(product) - quantity;
		}

		/// <summary>
		/// Checks the quantity of the specified product
		/// </summary>
		/// <returns>
		/// The quantity.
		/// </returns>
		/// <param name='product'>
		/// Product to check
		/// </param>
		public int CheckQuantity(Product product){
			int currentQuantity = 0;
			QuantityByProduct.TryGetValue(product, out currentQuantity);
			return currentQuantity;
		}

		/// <summary>
		/// Determines whether this instance can remove the specified product quantity.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can remove the specified product quantity; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='product'>
		/// If set to <c>true</c> product.
		/// </param>
		/// <param name='quantity'>
		/// If set to <c>true</c> quantity.
		/// </param>
		public bool CanRemove(Product product, int quantity){
			int currentQuantity = CheckQuantity(product);

			return (currentQuantity >= quantity);
		}
	}
}

