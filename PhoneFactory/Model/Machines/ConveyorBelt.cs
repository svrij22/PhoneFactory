using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class ConveyorBelt : FSMachine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Conveyor belt";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedSending { get; set; }
        public ConveyorBelt()
        {
            //Regular loop
            States.Add(new State()
            {
                Identifier = MachineState.PoweredOn,
                Name = "Powered on. Pending.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;

                    //has items to send?
                    if (QueuedItems > 0)
                        return MachineState.Sending;

                    return MachineState.PoweredOn;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.Sending,
                Name = "Sending",
                GetNext = () =>
                {

                    //Send to next machine
                    if (NextMachine != null)
                    {
                        NextMachine.QueuedItems += QueuedItems;
                        QueuedItems = 0;
                    }

                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.PoweredOn;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.PoweredOff,
                Name = "Powered off.",
                GetNext = () =>
                {
                    if (Factory.IsTurnedOn)
                        return MachineState.PoweredOn;
                    return MachineState.PoweredOff;
                }
            });


            //Set first state
            Current = States.First(state => state.Identifier == MachineState.PoweredOff);
        }
    }
}
