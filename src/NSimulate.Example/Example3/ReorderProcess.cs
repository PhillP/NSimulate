using System;
using NSimulate;
using NSimulate.Instruction;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate.Example3
{
	/// <summary>
	/// The reorder process.
	/// </summary>
	public class ReorderProcess : Process
	{
		private Product _product;

		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.ReorderProcess"/> class.
		/// </summary>
		/// <param name='product'>
		/// Product to be reordered
		/// </param>
		public ReorderProcess (Product product)
		{
			_product = product;
		}

		/// <summary>
		///  Simulate the process. 
		/// </summary>
		public override IEnumerator<InstructionBase> Simulate()
		{
			// wait for the reorder time appropriate for the product
			yield return new WaitInstruction(_product.ReorderTime);

			// increase the inventory level of this product
			var inventory = Context.GetByType<WarehouseInventory>().First();
			inventory.Add(_product, _product.ReorderAmount);
		}
	}
}

