using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class PackagingMachine : FSMachine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Packaging machine";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}/{StartedItems}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedItems { get; set; }
        public PackagingMachine()
        {

            States.Add(new State()
            {
                Identifier = MachineState.PoweredOn,
                Name = "Powered on. Ready.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;

                    if (StartedItems > 0)
                        return MachineState.Resuming;

                    if (QueuedItems > 0)
                    {
                        StartedItems += QueuedItems;
                        QueuedItems = 0;
                        return MachineState.PackagingPhone;
                    }

                    return MachineState.PoweredOn;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.Resuming,
                Name = "Resuming",
                GetNext = () =>
                {
                    return MachineState.PackagingPhone;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.PackagingPhone,
                Name = "Packaging phone",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.WrappingProducts;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.WrappingProducts,
                Name = "Wrapping products",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.MarkingPhoneGUID;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.MarkingPhoneGUID,
                Name = "Marking Phone GUID",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.SendToBelt;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.SendToBelt,
                Name = "Sending to conveyor belt",
                GetNext = () =>
                {

                    //Send to next machine
                    if (NextMachine != null)
                        NextMachine.QueuedItems += StartedItems;
                    StartedItems = 0;

                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.PoweredOn;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.PoweredOff,
                Name = "Powered off",
                GetNext = () =>
                {
                    if (Factory.IsTurnedOn)
                        return MachineState.PoweredOn;
                    return MachineState.PoweredOff;
                }
            });

            //Set first state
            Current = States.First(state => state.Identifier == MachineState.PoweredOn);
        }
    }
}
