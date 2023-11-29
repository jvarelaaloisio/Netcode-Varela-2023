using System;

namespace IA.FSM
{
    public interface IState<T>
    {
        /// <summary>
        /// The name of this state
        /// </summary>
        string name { get; }

        /// <summary>
        /// Called once when the FSM enters the State
        /// </summary>
        event Action OnAwake;

        /// <summary>
        /// Called once when the FSM exits the State
        /// </summary>
        event Action OnSleep;

        /// <summary>
        /// Method called once when entering this state and after exiting the last one.
        /// Always call base method so the corresponding event is raised
        /// </summary>
        void HandleAwake();

        /// <summary>
        /// Method called once when exiting this state and before entering another.
        /// Always call base method so the corresponding event is raised
        /// </summary>
        void HandleSleep();

        void AddTransition(T key, IState<T> transition);
        bool TryGetTransition(T key, out IState<T> transition);
    }
}