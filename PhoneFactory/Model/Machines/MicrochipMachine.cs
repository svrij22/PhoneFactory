using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class MicrochipMachine : FSMachine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Microchip machine";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}/{StartedItems}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedItems { get; set; }
        public MicrochipMachine()
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
                        return MachineState.PreparingMicrochips;
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
                    return MachineState.PreparingMicrochips;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.PreparingMicrochips,
                Name = "Preparing microchips",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.EmbeddingCases;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.EmbeddingCases,
                Name = "Embedding cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.PoweredOff;
                    return MachineState.AttachWiring;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.AttachWiring,
                Name = "Attaching wiring",
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
