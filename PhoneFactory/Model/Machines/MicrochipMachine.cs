using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class MicrochipMachine : Machine
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

            MyStates.Add(new State()
            {
                This = StateNames.PoweredOn,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Powered on. Ready.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;

                    if (QueuedItems > 0)
                    {
                        StartedItems += QueuedItems;
                        QueuedItems = 0;
                        return StateNames.PreparingMicrochips;
                    }

                    return StateNames.PoweredOn;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.PreparingMicrochips,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Preparing microchips",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;
                    return StateNames.EmbeddingCases;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.EmbeddingCases,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Embedding cases",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;
                    return StateNames.AttachWiring;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.AttachWiring,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Attaching wiring",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;
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
                        NextMachine.QueuedItems += StartedItems;
                    StartedItems = 0;

                    if (!Factory.IsTurnedOn)
                        return StateNames.PoweredOff;
                    return StateNames.PoweredOn;
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

            //Set first state
            Current = MyStates.First(state => state.This == StateNames.PoweredOn);
        }
    }
}
