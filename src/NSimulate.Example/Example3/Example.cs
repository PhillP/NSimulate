using System;
using System.Collections.Generic;
using NSimulate;
using System.Linq;

namespace NSimulate.Example3
{
	public class Example
	{
		public static void Run(){
			// Make a simulation context
			using (var context = new SimulationContext(isDefaultContextForProcess: true))
			{
				var products = CreateProducts();

				var inventory = CreateWarehouseInventory(products);
				context.Register<WarehouseInventory>(inventory);

				var orders = GenerateOrders(500, products);
    			var deliveryPeople = CreateDeliveryPeople(3, orders);

				// instantate a new simulator
				var simulator = new Simulator();

				// run the simulation
				simulator.Simulate();

				// output the statistics
				OutputResults(deliveryPeople);
			}
		}

		private static IEnumerable<Product> CreateProducts() {

			return new List<Product>(){
				new Product(){ ReorderCount = 90, ReorderAmount = 200, ReorderTime=500 },
				new Product(){ ReorderCount = 110, ReorderAmount = 170, ReorderTime=900 },
				new Product(){ ReorderCount = 45, ReorderAmount = 70, ReorderTime=300 },
			};
		}

		private static IEnumerable<DeliveryPerson> CreateDeliveryPeople(int numberOfDeliveryPeople, OrderQueue queue) {
			var deliveryPeople =  new List<DeliveryPerson>();

			for(int i = 0; i < numberOfDeliveryPeople; i++){
				deliveryPeople.Add(new DeliveryPerson(queue));
			}

			return deliveryPeople;
		}

		private static WarehouseInventory CreateWarehouseInventory(IEnumerable<Product> products) {
			var inventory = new WarehouseInventory();

			// start with an inventory of twice the reorder level of each product
			foreach(var product in products){
				inventory.Add(product, product.ReorderCount * 2);
			}
			return inventory;
		}

		private static OrderQueue GenerateOrders(int numberOfOrders, IEnumerable<Product> products) {

			var rand = new Random();

			int productCount = products.Count();
			OrderQueue queue = new OrderQueue();

			for (int i=0;i<numberOfOrders;i++){
				var product = products.ElementAt(rand.Next(productCount));
				queue.Enqueue(new Order(){
			 		Product = product,
					Quantity = rand.Next(10),
					DeliveryAddress = new Address()
				});
			}

			return queue;
		}

		private static void OutputResults(IEnumerable<DeliveryPerson> deliveryPeople){
			int index = 1;
			foreach(var deliveryPerson in deliveryPeople){
				Console.WriteLine(string.Format("Delivery Person: {0}, Deliveries: {1}, Busy Time: {2}, Wait Time: {3}, Latest Delivery Time: {4}", index, deliveryPerson.DeliveryCount, deliveryPerson.BusyTime, deliveryPerson.WaitTime, deliveryPerson.LatestDeliveryTime));
				index++;
			}
		}
	}
}