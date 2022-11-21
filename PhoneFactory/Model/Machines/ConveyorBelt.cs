using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class ConveyorBelt : Machine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Conveyor belt";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}/{StartedSending}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedSending { get; set; }
        public ConveyorBelt()
        {
            //Regular loop
            MyStates.Add(new State()
            {
                This = StateNames.PoweredOn,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Powered on. Pending.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;

                    //has items to send?
                    if (QueuedItems > 0)
                    {
                        //Set started
                        StartedSending += QueuedItems;
                        QueuedItems = 0;
                        return StateNames.Sending;
                    }

                    return StateNames.PoweredOn;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.Sending,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Sending",
                GetNext = () =>
                {

                    //Send to next machine
                    if (NextMachine != null)
                        NextMachine.QueuedItems += StartedSending;
                    StartedSending = 0;

                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;
                    return StateNames.PoweredOn;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.PoweredOff,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Powered off.",
                GetNext = () =>
                {
                    if (Factory.IsTurnedOn)
                        return StateNames.PoweredOn;
                    return StateNames.PoweredOff;
                }
            });


            //Set first state
            Current = MyStates.First(state => state.This == StateNames.PoweredOff);
        }
    }
}
