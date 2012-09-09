using System;
using System.Collections.Generic;
using NSimulate.Instruction;
using NSimulate;
using System.Linq;

namespace NSimulate.Example3
{
	/// <summary>
	/// The process of delivery perfomed by a delivery person.
	/// </summary>
	public class DeliveryPerson : Process
	{
		private OrderQueue _orderQueue;

		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.Example3.DeliveryPerson"/> class.
		/// </summary>
		/// <param name='orderQueue'>
		/// Order queue.
		/// </param>
		public DeliveryPerson (OrderQueue orderQueue)
		{
			_orderQueue = orderQueue;
		}

		/// <summary>
		/// Gets the cumulative wait time (in time periods) that this delivery person has waited
		/// </summary>
		/// <value>
		/// The wait time.
		/// </value>
		public int WaitTime{
			get;
			private set;
		}

		/// <summary>
		/// Gets the busy time (in time periods) of this delivery person.
		/// </summary>
		/// <value>
		/// The busy time.
		/// </value>
		public int BusyTime{
			get;
			private set;
		}

		/// <summary>
		/// Gets the delivery count (number of orders delivered).
		/// </summary>
		/// <value>
		/// The delivery count.
		/// </value>
		public int DeliveryCount{
			get;
			private set;
		}

		/// <summary>
		/// Gets the latest delivery time.
		/// </summary>
		/// <value>
		/// The latest delivery time.
		/// </value>
		public int LatestDeliveryTime{
			get;
			private set;
		}

		/// <summary>
		///  Simulate the process. 
		/// </summary>
		public override IEnumerator<InstructionBase> Simulate()
		{
			var inventory = Context.GetByType<WarehouseInventory>().First();

			int waitStart = Context.TimePeriod;

			while(_orderQueue.Count >0)
			{
				if (Context.TimePeriod > waitStart){
					WaitTime += (Context.TimePeriod - waitStart);
					waitStart = Context.TimePeriod;
				}

				Order orderToDeliver = null;

				int i = 0;
				while(i < _orderQueue.Count && orderToDeliver == null){
					var orderToCheck = _orderQueue.Dequeue();
					if (inventory.CanRemove(orderToCheck.Product, orderToCheck.Quantity)){
						orderToDeliver = orderToCheck;
					}
					else{
						// the order can't be filled yet.. push it to the end of the queue
						_orderQueue.Enqueue(orderToCheck);
					}
					i++;
				}

				if (orderToDeliver == null){
					// can't deliver anything now... pass
					yield return new PassInstruction();
				}
				else {
					// remove from the warehouse
					inventory.Remove(orderToDeliver.Product, orderToDeliver.Quantity);

					// check the remaining quantity
					int quantity = inventory.CheckQuantity(orderToDeliver.Product);

					if (quantity <= orderToDeliver.Product.ReorderCount 
					    && (quantity + orderToDeliver.Quantity) > orderToDeliver.Product.ReorderCount){
						// the recent removal pushed levels below the reorder count
						// start the reorder process
						var reorder = new ReorderProcess(orderToDeliver.Product);
						yield return new ActivateInstruction(reorder);
					}
					// deliver the order
					// work out how far to travel
					int distanceToTravel = (int)Math.Sqrt((orderToDeliver.DeliveryAddress.X * orderToDeliver.DeliveryAddress.X)
					                                 + (orderToDeliver.DeliveryAddress.Y * orderToDeliver.DeliveryAddress.Y));

					// include travel back to warehouse
					distanceToTravel = distanceToTravel * 2;

					int timeToTravel = (int)(distanceToTravel / 10);

					var deliveryStartTime = Context.TimePeriod;
					// wait the delivery time
					yield return new WaitInstruction(timeToTravel);
					var deliveryEndTime = Context.TimePeriod;

					BusyTime += (deliveryEndTime - deliveryStartTime);
					DeliveryCount++;
					waitStart = Context.TimePeriod;
					LatestDeliveryTime = Context.TimePeriod;
				}
			}
		}
	}
}

