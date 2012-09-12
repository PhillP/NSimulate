using System;
using System.Collections.Generic;
using NSimulate;

namespace NSimulate.Example2
{
	/// <summary>
	/// An example simulation
	/// </summary>
	public class Example
	{
		/// <summary>
		/// Run this example
		/// </summary>
		public static void Run(){
			// Make a simulation context
			using (var context = new SimulationContext(isDefaultContextForProcess: true))
			{
				// Add the resources that represent the staff of the call center
				context.Register<Level1CallCenterStaffMember>(new Level1CallCenterStaffMember(){ Capacity = 10 });
				context.Register<Level2CallCenterStaffMember>(new Level2CallCenterStaffMember(){ Capacity = 5 });

				// Add the processes that represent the phone calls to the call center
				IEnumerable<Call> calls = GeneratePhoneCalls(context,
					numberOfCalls: 500,
				    level1CallTime: 120,
				    level2CallTime: 300,
					callTimeVariability: 0.5,
				    callStartTimeRange: 14400);
                
				// instantate a new simulator
				var simulator = new Simulator();

				// run the simulation
				simulator.Simulate();

				// output the statistics
				OutputResults(calls);
			}
		}

		/// <summary>
		/// Generates the phone calls for the simulation
		/// </summary>
		/// <returns>
		/// The phone calls.
		/// </returns>
		/// <param name='context'>
		/// Context.
		/// </param>
		/// <param name='numberOfCalls'>
		/// Number of calls.
		/// </param>
		/// <param name='level1CallTime'>
		/// Level1 call time.
		/// </param>
		/// <param name='level2CallTime'>
		/// Level2 call time.
		/// </param>
		/// <param name='callTimeVariability'>
		/// Call time variability.
		/// </param>
		/// <param name='callStartTimeRange'>
		/// Call start time range.
		/// </param>
		private static IEnumerable<Call> GeneratePhoneCalls(SimulationContext context, 
			int numberOfCalls,
		    int level1CallTime,
			int level2CallTime,
			double callTimeVariability,
			int callStartTimeRange) {

			var rand = new Random();
			List <Call> calls = new List<Call>();

			for(int i =0;i<numberOfCalls;i++){

				CallCharacteristics characteristics = new CallCharacteristics();

				characteristics.TimeBeforeCallStarts = rand.Next(callStartTimeRange);

				// for half the calls, force them to be in a peak period in the middle of the range
				if (i < numberOfCalls / 2){
					characteristics.TimeBeforeCallStarts = (int)((characteristics.TimeBeforeCallStarts/3.0) + (callStartTimeRange / 3.0));
				}

				characteristics.CallDurationAtLevel1 = (int)(level1CallTime 
					+ rand.Next((int)(level1CallTime*callTimeVariability))
						- ((level1CallTime* callTimeVariability)/2.0));

				characteristics.CallIsEscalatedToLevel2 = (rand.Next(1000)>500);
				if (characteristics.CallIsEscalatedToLevel2){
					characteristics.CallDurationAtLevel2 = (int)(level1CallTime 
					+ rand.Next((int)(level1CallTime*callTimeVariability))
						- ((level1CallTime* callTimeVariability)/2.0));

				}

				var newCall = new Call(characteristics);
				calls.Add(newCall);
			}

			return calls;
		}

		/// <summary>
		/// Outputs the results using statistics captured for each call
		/// </summary>
		/// <param name='calls'>
		/// Calls.
		/// </param>
		private static void OutputResults(IEnumerable<Call> calls){
			int countOfCalls=0;
			int countOfCallsReachingLevel2=0;
			long totalLevel1Duration=0;
			long totalLevel2Duration=0;
			long totalHoldTimeBeforeLevel1=0;
			long totalHoldTimeBeforeLevel2=0;
			long totalCallTime=0;
			long totalHoldTime=0;
			long? maxHoldTimeBeforeLevel1=null;
			long? maxHoldTimeBeforeLevel2=null;
			long? maxTotalHoldTime=null;
			long? maxTotalCallTime=null;

			foreach(Call call in calls){
				countOfCalls++;
				if (call.Statistics.CallLevel2TimePeriod != null){
					countOfCallsReachingLevel2++;
				}

				totalLevel1Duration += call.Statistics.TimeAtLevel1 ?? 0;
				totalLevel2Duration += call.Statistics.TimeAtLevel2 ?? 0;
				totalHoldTimeBeforeLevel1 += call.Statistics.TimeOnHoldBeforeLevel1 ?? 0;
				totalHoldTimeBeforeLevel2 += call.Statistics.TimeOnHoldBeforeLevel2 ?? 0;
				totalCallTime += call.Statistics.TotalTimeOfCall ?? 0;
				totalHoldTime += call.Statistics.TotalTimeOnHold ?? 0;

				if (maxHoldTimeBeforeLevel1 == null 
				    || call.Statistics.TimeOnHoldBeforeLevel1 > maxHoldTimeBeforeLevel1) {
					maxHoldTimeBeforeLevel1 = call.Statistics.TimeOnHoldBeforeLevel1;
				}

				if (maxHoldTimeBeforeLevel2 == null 
				    || call.Statistics.TimeOnHoldBeforeLevel2 > maxHoldTimeBeforeLevel2) {
					maxHoldTimeBeforeLevel2 = call.Statistics.TimeOnHoldBeforeLevel2;
				}

				if (maxTotalHoldTime == null 
				    || call.Statistics.TotalTimeOnHold > maxTotalHoldTime) {
					maxTotalHoldTime = call.Statistics.TotalTimeOnHold;
				}

				if (maxTotalCallTime == null 
				    || call.Statistics.TotalTimeOfCall > maxTotalCallTime) {
					maxTotalCallTime = call.Statistics.TotalTimeOfCall;
				}
			}

			if (countOfCalls > 0){
				Console.WriteLine("------------------------------------------------------");
				Console.WriteLine(string.Format("Number of calls                 : {0}", countOfCalls));
				Console.WriteLine(string.Format("Average hold time before level 1: {0} seconds", totalHoldTimeBeforeLevel1/countOfCalls));
				Console.WriteLine(string.Format("Maximum hold time before level 1: {0} seconds", maxHoldTimeBeforeLevel1));
				Console.WriteLine(string.Format("Average call time at level 1    : {0} seconds", totalLevel1Duration/countOfCalls));
				Console.WriteLine("------------------------------------------------------");
				if (countOfCallsReachingLevel2 > 0){
					Console.WriteLine(string.Format("Number of calls reaching level 2: {0}", countOfCallsReachingLevel2));
					Console.WriteLine(string.Format("Average hold time before level 2: {0} seconds", totalHoldTimeBeforeLevel2/countOfCallsReachingLevel2));
					Console.WriteLine(string.Format("Maximum hold time before level 2: {0} seconds", maxHoldTimeBeforeLevel2));
					Console.WriteLine(string.Format("Average call time at level 2    : {0} seconds", totalLevel2Duration/countOfCallsReachingLevel2));
					Console.WriteLine("------------------------------------------------------");
				}
				Console.WriteLine(string.Format("Total call time                 : {0} seconds", totalCallTime));
				Console.WriteLine(string.Format("Total hold time                 : {0} seconds", totalHoldTime));
				Console.WriteLine(string.Format("Average call time               : {0} seconds", totalCallTime/countOfCalls));
				Console.WriteLine(string.Format("Average hold time               : {0} seconds", totalHoldTime/countOfCalls));
				Console.WriteLine("------------------------------------------------------");
			}
		}
	}
}
