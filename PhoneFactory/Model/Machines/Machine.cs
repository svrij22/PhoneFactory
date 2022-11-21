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
    public abstract class Machine : INotifyPropertyChanged
    {
        public int QueuedItems { get; set; }
        public abstract string Name { get; set; } 
        public abstract string FormattedState { get; }
        public abstract string Information { get; }
        public List<State> MyStates { get; set; } = new();
        public State Current { get; set; }

        public Timer MachineTimer;
        public void Start()
        {
            MachineTimer = new();
            MachineTimer.Interval = Current.TimeSpan.Seconds * 1000;
            MachineTimer.Enabled = true;
            MachineTimer.Elapsed += (s, e) =>
            {
                StateNames next = Current.GetNext();
                Current = MyStates.First(state => state.This == next);

                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(FormattedState));
                OnPropertyChanged(nameof(Information));
                MachineTimer.Interval = Current.TimeSpan.Milliseconds;
            };
            MachineTimer.Start();
        }

        public Machine NextMachine { get; set; }

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
