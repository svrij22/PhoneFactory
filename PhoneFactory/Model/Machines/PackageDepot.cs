using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class PackageDepot : FSMachine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Package depot";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedSending { get; set; }
        public PackageDepot()
        {
            //Regular loop
            States.Add(new State()
            {
                Identifier = MachineState.PoweredOn,
                TimeSpan = TimeSpan.FromMilliseconds(550),
                Name = "Package depot.",
                GetNext = () =>
                {
                    return MachineState.PoweredOn;
                }
            });

            //Set first state
            Current = States.First(state => state.Identifier == MachineState.PoweredOn);
        }
    }
}
