using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class MetalEncasingMachine : FSMachine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Metal encasing machine";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}/{StartedCases}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedCases { get; set; }

        public int RemoveFromQueue(int max)
        {
            var amt = Math.Min(max, QueuedItems);
            QueuedItems -= amt;
            return amt;
        }
        public MetalEncasingMachine()
        {
            //Regular loop
            States.Add(new State()
            {
                Identifier = MachineState.WarmingUpOven,
                Name = "Warming Oven Up",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
                    return MachineState.WarmedUp;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.WarmedUp,
                Name = "Powered up. Ready.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
                    if (QueuedItems > 0)
                        return MachineState.MeltingMetal;
                    if (StartedCases > 0)
                        return MachineState.Resuming;
                    return MachineState.WarmedUp;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.Resuming,
                Name = "Resuming",
                GetNext = () =>
                {
                    return MachineState.MeltingMetal;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.MeltingMetal,
                Name = "Melting metal",
                GetNext = () =>
                {
                    //Set started
                    StartedCases += RemoveFromQueue(8);

                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
                    return MachineState.MoldingMetal;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.MoldingMetal,
                Name = "Molding into cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
                    return MachineState.CoolingCases;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.CoolingCases,
                Name = "Cooling down cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
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
                        NextMachine.QueuedItems += StartedCases;
                    StartedCases = 0;

                    if (!Factory.IsTurnedOn)
                        return MachineState.CoolingDown;
                    return MachineState.WarmedUp;
                }
            });

            States.Add(new State()
            {
                Identifier = MachineState.CoolingDown,
                Name = "Cooling down machine",
                GetNext = () =>
                {
                    return MachineState.PoweredOff;
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

            States.Add(new State()
            {
                Identifier = MachineState.PoweredOn,
                Name = "Powering on machine",
                GetNext = () =>
                {
                    return MachineState.WarmedUp;
                }
            });


            //Set first state
            Current = States.First(state => state.Identifier == MachineState.PoweredOff);
        }
    }
}
