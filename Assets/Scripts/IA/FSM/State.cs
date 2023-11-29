using System;
using System.Collections.Generic;

namespace IA.FSM
{
	/// <summary>
	/// State used by the Finite State Machine (FSM) Class
	/// </summary>
	public class State<T> : IState<T>
	{
		private readonly Dictionary<T, IState<T>> _transitions = new Dictionary<T, IState<T>>();

		public static implicit operator bool(State<T> state) => state != null;

		/// <summary>
		/// Called once when the FSM enters the State
		/// </summary>
		public event Action OnAwake = delegate { };

		/// <summary>
		/// Called once when the FSM exits the State
		/// </summary>
		public event Action OnSleep = delegate { };
		
		public virtual string name { get; }

		public State(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Method called once when entering this state and after exiting the last one.
		/// Always call base method so the corresponding event is raised
		/// </summary>
		public virtual void HandleAwake()
			=> OnAwake();

		/// <summary>
		/// Method called once when exiting this state and before entering another.
		/// Always call base method so the corresponding event is raised
		/// </summary>
		public virtual void HandleSleep()
			=> OnSleep();

		public void AddTransition(T key, IState<T> transition)
		{
			if (!_transitions.ContainsKey(key))
				_transitions.Add(key, transition);
		}

		public bool TryGetTransition(T key, out IState<T> transition)
			=> _transitions.TryGetValue(key, out transition);
	}
}