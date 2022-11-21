using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class MetalEncasingMachine : Machine
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
        public MetalEncasingMachine()
        {
            //Regular loop
            MyStates.Add(new State()
            {
                This = StateNames.WarmingUpOven,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Warming Oven Up",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    return StateNames.WarmedUp;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.WarmedUp,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Warmed up. Ready.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    if (QueuedItems > 0)
                        return StateNames.MeltingMetal;
                    return StateNames.WarmedUp;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.MeltingMetal,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Melting metal",
                GetNext = () =>
                {
                    //Set started
                    StartedCases += QueuedItems;
                    QueuedItems = 0;

                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    return StateNames.MoldingMetal;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.MoldingMetal,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Molding into cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    return StateNames.CoolingCases;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.CoolingCases,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Cooling down cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    return StateNames.SendToBelt;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.SendToBelt,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Sending to conveyor belt",
                GetNext = () =>
                {

                    //Send to next machine
                    if (NextMachine != null)
                        NextMachine.QueuedItems += StartedCases;
                    StartedCases = 0;

                    if (!Factory.IsTurnedOn)
                        return StateNames.CoolingDown;
                    return StateNames.WarmedUp;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.CoolingDown,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Cooling down machine",
                GetNext = () =>
                {
                    return StateNames.PoweredOff;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.PoweredOff,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Powered off",
                GetNext = () =>
                {
                    if (Factory.IsTurnedOn)
                        return StateNames.PoweredOn;
                    return StateNames.PoweredOff;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.PoweredOn,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Powering on machine",
                GetNext = () =>
                {
                    return StateNames.WarmedUp;
                }
            });


            //Set first state
            Current = MyStates.First(state => state.This == StateNames.PoweredOff);
        }
    }
}
