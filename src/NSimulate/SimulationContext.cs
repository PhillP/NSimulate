using System;
using System.Collections.Generic;
using System.Linq;

namespace NSimulate
{
	/// <summary>
	/// Context containing state information for a simulation
	/// </summary>
	public class SimulationContext : IDisposable
	{
		private static SimulationContext _current;

		private Dictionary<Type, Dictionary<object, SimulationElement>> _registeredElements = new Dictionary<Type, Dictionary<object, SimulationElement>>();

		/// <summary>
		/// Gets the current SimulationContext.  A new SimulationContext is created if one does not already exist.
		/// </summary>
		/// <value>
		/// The current simulation context
		/// </value>
		public static SimulationContext Current
		{
			get
			{
				return _current;
			}
			private set
			{
				_current = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.SimulationContext"/> class.
		/// </summary>
		public SimulationContext ()
			: this(false)
		{
		}

		/// <summary>
		/// Gets or sets the time period.
		/// </summary>
		/// <value>
		/// The time period.
		/// </value>
		public int TimePeriod {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the simulation is terminating.
		/// </summary>
		/// <value>
		/// <c>true</c> if the simulation terminating; otherwise, <c>false</c>.
		/// </value>
		public bool IsSimulationTerminating{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether the simulation is stopping.
		/// </summary>
		/// <value>
		/// <c>true</c> if the simulation is stopping; otherwise, <c>false</c>.
		/// </value>
		public bool IsSimulationStopping{
			get;
			set;
		}

		/// <summary>
		/// Gets the active processes.
		/// </summary>
		/// <value>
		/// The active processes.
		/// </value>
		public HashSet<Process> ActiveProcesses{
			get;
			private set;
		}

		/// <summary>
		/// Gets the processes remaining this time period.
		/// </summary>
		/// <value>
		/// The processes remaining this time period.
		/// </value>
		public Queue<Process> ProcessesRemainingThisTimePeriod{
			get;
			private set;
		}

		/// <summary>
		/// Gets the processed processes.
		/// </summary>
		/// <value>
		/// The processed processes.
		/// </value>
		public HashSet<Process> ProcessedProcesses{
			get;
			private set;
		}

		/// <summary>
		/// Moves to time period.
		/// </summary>
		/// <param name='timePeriod'>
		/// Time period.
		/// </param>
		public void MoveToTimePeriod(int timePeriod){

			TimePeriod = timePeriod;

			if (ActiveProcesses == null){
				ActiveProcesses = new HashSet<Process>();

				// order processes by instruction and then process priority
				var processesInPriorityOrder = GetByType<Process>()
					.OrderBy(p=>p.Priority)
					.ThenBy(p=>(p.SimulationState != null 
					             && p.SimulationState.InstructionEnumerator != null 
					             && p.SimulationState.InstructionEnumerator.Current != null)
					         ?p.SimulationState.InstructionEnumerator.Current.Priority
					         :Priority.Medium)
					.ThenBy(p=>p.InstanceIndex);
					
				foreach(var process in processesInPriorityOrder){
					if (process.SimulationState == null){
						process.SimulationState = new ProcessSimulationState();
					}

					if (process.SimulationState.IsActive){
						ActiveProcesses.Add(process);
					}
				}
			}

			ProcessesRemainingThisTimePeriod = new Queue<Process>();
			foreach(var process in ActiveProcesses){
				ProcessesRemainingThisTimePeriod.Enqueue(process);
			}
			ProcessedProcesses = new HashSet<Process>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NSimulate.SimulationContext"/> class.
		/// </summary>
		public SimulationContext (bool isDefaultContextForProcess)
		{
			if (isDefaultContextForProcess)
			{
				_current = this;
			}
		}

		/// <summary>
		/// Register an object with this context
		/// </summary>
		public void Register<TType>(TType objectToRegister)
			where TType : SimulationElement
		{
			Register(typeof(TType), objectToRegister);
		}

		/// <summary>
		/// Register an object with this context
		/// </summary>
		public void Register(Type typeToRegister, object objectToRegister)
		{
			if (!typeToRegister.IsSubclassOf(typeof(SimulationElement)))
			{
				throw new ArgumentException("typeToRegister");
			}

			SimulationElement element = objectToRegister as SimulationElement;

			Dictionary<object, SimulationElement> elementsByKey = null;

			if (!_registeredElements.TryGetValue(typeToRegister, out elementsByKey))
			{
				elementsByKey = new Dictionary<object, SimulationElement>();
				_registeredElements[typeToRegister] = elementsByKey;
			}

			elementsByKey[element.Key] = element;
		}

		/// <summary>
		/// Gets a previously registered object by key.
		/// </summary>
		/// <returns>
		/// The matching object if any
		/// </returns>
		/// <param name='key'>
		/// Key identifying object
		/// </param>
		/// <typeparam name='TType'>
		/// The type of object to retrieve
		/// </typeparam>
		public TType GetByKey<TType>(object key)
			where TType : SimulationElement
		{
			SimulationElement objectToRetrieve = null;

			foreach(var entry  in _registeredElements) {
				if (entry.Key == typeof(TType) || entry.Key.IsSubclassOf(typeof(TType))){
					if (entry.Value.TryGetValue(key, out objectToRetrieve)){
						break;
					}
				}
			}
		
			return objectToRetrieve as TType;
		}

		/// <summary>
		/// Get a list of objects registered
		/// </summary>
		/// <returns>
		/// A list of matching objects
		/// </returns>
		/// <typeparam name='TType'>
		/// The type of objects to retrieve
		/// </typeparam>
		public IEnumerable<TType> GetByType<TType>()
		{
			List<TType> enumerableToReturn = new List<TType>();

			foreach(var entry  in _registeredElements) {
				if (entry.Key == typeof(TType) || entry.Key.IsSubclassOf(typeof(TType))){
					enumerableToReturn.AddRange(entry.Value.Values.Cast<TType>());
				}
			}
		
			return enumerableToReturn;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>
		/// 2
		/// </filterpriority>
		/// <remarks>
		/// Call <see cref="Dispose"/> when you are finished using the <see cref="NSimulate.SimulationContext"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="NSimulate.SimulationContext"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="NSimulate.SimulationContext"/> so
		/// the garbage collector can reclaim the memory that the <see cref="NSimulate.SimulationContext"/> was occupying.
		/// </remarks>
		public void Dispose()
		{
			if (SimulationContext.Current == this)
			{
				SimulationContext.Current = null;
			}
		}
	}
}

