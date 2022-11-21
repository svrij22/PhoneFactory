using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PhoneFactory.Machines
{
    public abstract class FSMachine : INotifyPropertyChanged
    {
        /// <summary>
        /// Use-case specific
        /// </summary>
        public int QueuedItems { get; set; }

        /// <summary>
        /// UI Formatting
        /// </summary>
        public abstract string Name { get; set; }
        public abstract string FormattedState { get; }
        public abstract string Information { get; }

        /// <summary>
        /// State machine
        /// </summary>
        public List<State> States { get; set; } = new();
        public State Current { get; set; }

        public static Timer MachineTimer = new();
        public void Start()
        {
            MachineTimer.Interval = Current.TimeSpan.Milliseconds;
            MachineTimer.Enabled = true;
            MachineTimer.Elapsed += (s, e) =>
            {
                var prevState = Current.Identifier;
                var nextState = Current.GetNext();
                Current = States.First(state => state.Identifier == nextState);

                if (prevState != nextState)
                {
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(FormattedState));
                }

                OnPropertyChanged(nameof(Information));
            };
            MachineTimer.Start();
        }

        public FSMachine NextMachine { get; set; }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
