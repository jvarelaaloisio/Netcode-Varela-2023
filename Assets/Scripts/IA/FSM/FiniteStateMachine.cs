using System;
using UnityEngine;

namespace IA.FSM
{
	/// <summary>
	/// Finite state machine
	/// </summary>
	/// <typeparam name="T">The key Type to access the different states</typeparam>
	public class FiniteStateMachine<T>
	{
		private readonly string _tag;
		private bool _shouldLogTransitions = false;

		private ILogger _logger;

		/// <summary>
		/// Current state running in the FSM
		/// </summary>
		public IState<T> CurrentState { get; private set; }

		/// <summary>
		/// Event triggered when the state changes. First state is the last and second is the new one.
		/// </summary>
		public event Action<IState<T>, IState<T>> OnTransition = delegate { };

		private FiniteStateMachine(IState<T> initialState,
		                           string ownerTag = "")
		{
			CurrentState = initialState;
			CurrentState.HandleAwake();
			_tag = ownerTag != "" ? $"<b>{ownerTag} (FSM)</b>" : "<b>FSM</b>";
		}

		/// <summary>
		/// Change the current state to another one in the dictionary.
		/// </summary>
		/// <param name="key">Key for the next state</param>
		public void TransitionTo(T key)
		{
			if (!CurrentState.TryGetTransition(key, out var transition))
				return;

			if (transition == CurrentState)
				return;

			CurrentState?.HandleSleep();

			if (_shouldLogTransitions)
				_logger.Log(_tag, $"changed state: {CurrentState.name} -> {transition.name}");

			CurrentState = transition;
			CurrentState.HandleAwake();
			OnTransition(CurrentState, transition);
		}

		/// <summary>
		/// Builder method to simplify code.
		/// </summary>
		/// <param name="initialState">Base state for the FSM</param>
		/// <param name="ownerTag">Used for logging</param>
		/// <returns>Builder class (Use method Done to get the desired state machine once the setup is completed).</returns>
		public static Builder Build(IState<T> initialState,
									string ownerTag = "")
			=> new Builder(initialState,
							ownerTag);

		public class Builder
		{
			private readonly FiniteStateMachine<T> _finiteStateMachine;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="initialState">Base state for the FSM</param>
			/// <param name="ownerTag">Used for logging</param>
			internal Builder(IState<T> initialState,
							string ownerTag = "")
				=> _finiteStateMachine = new FiniteStateMachine<T>(initialState,
																	ownerTag);

			public FiniteStateMachine<T> Done()
				=> _finiteStateMachine;

			public Builder WithThisLogger(ILogger logger)
			{
				_finiteStateMachine._logger = logger;
				return this;
			}

			public Builder ThatLogsTransitions(bool value = true)
			{
				if (value)
				{
					if (_finiteStateMachine._logger == null)
					{
						throw new
							ArgumentException("FSM should have a logger set." +
											" Please use the method <b>WithThisLogger(<i>Logger</i>)</b> before calling this one");
					}

					_finiteStateMachine._logger.Log(_finiteStateMachine._tag, "Now this FSM logs transitions");
				}

				_finiteStateMachine._shouldLogTransitions = value;
				return this;
			}

			public Builder ThatTriggersOnTransition(Action<IState<T>, IState<T>> eventHandler)
			{
				_finiteStateMachine.OnTransition += eventHandler;
				return this;
			}
		}
	}
}