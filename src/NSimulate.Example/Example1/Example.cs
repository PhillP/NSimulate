using System;
using NSimulate;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace NSimulate.Example1
{
	/// <summary>
	/// A simulation example.
	/// </summary>
	public static class Example
	{
		/// <summary>
		/// Run the example
		/// </summary>
		public static void Run(){
			// Make a simulation context
			using (var context = new SimulationContext(isDefaultContextForProcess: true))
			{
				// initialise the model
				CreateModel(numberOfJobs: 500);

				// instantate a new simulator
				var simulator = new Simulator();

				// run the simulation
				simulator.Simulate();

				Console.WriteLine("Jobs processed in {0} minutes", context.TimePeriod);
			}
		}

		/// <summary>
		/// Creates the model.
		/// </summary>
		/// <param name='numberOfJobs'>
		/// Number of jobs to be generated.
		/// </param>
		private static void CreateModel(int numberOfJobs){

			var rand = new Random();

			var unprocessedJobsList = new List<Job>();

			// Create job queues of various work types
			var workTypeAJobQueue = new Queue<Job>();
			var workTypeBJobQueue = new Queue<Job>();
			var workTypeCJobQueue = new Queue<Job>();
			var workQueues = new List<Queue<Job>>(){ workTypeAJobQueue, workTypeBJobQueue, workTypeCJobQueue };

			// create machines
			var machine1 = new Machine(jobQueue: workTypeAJobQueue, reliabilityPercentage: 95.0m, repairTimeRequired: 15, unprocessedJobsList: unprocessedJobsList);
			var machine2 = new Machine(jobQueue: workTypeAJobQueue, reliabilityPercentage: 80.0m, repairTimeRequired: 22, unprocessedJobsList: unprocessedJobsList);

			var machine3 = new Machine(jobQueue: workTypeBJobQueue, reliabilityPercentage: 99.0m, repairTimeRequired: 15, unprocessedJobsList: unprocessedJobsList);
			var machine4 = new Machine(jobQueue: workTypeBJobQueue, reliabilityPercentage: 35.0m, repairTimeRequired: 25, unprocessedJobsList: unprocessedJobsList);

			var machine5 = new Machine(jobQueue: workTypeCJobQueue, reliabilityPercentage: 90.0m, repairTimeRequired: 45, unprocessedJobsList: unprocessedJobsList);

			// create the jobs
			for (int i = 0; i< numberOfJobs;i++){
				var newJob = new Job();

				newJob.ProcessingTimeRequiredByJobQueue[workTypeAJobQueue] = 5 + rand.Next(5);
				newJob.ProcessingTimeRequiredByJobQueue[workTypeBJobQueue] = 5 + rand.Next(5);
				newJob.ProcessingTimeRequiredByJobQueue[workTypeCJobQueue] = 5 + rand.Next(5);

				int index = rand.Next(workQueues.Count);
				// enque the job in one of the work queues
				workQueues[index].Enqueue(newJob);

				// and add it to the unprocessed job list
				unprocessedJobsList.Add(newJob);
			}

			// add a repair person
			new RepairPerson();
		}
	}
}
