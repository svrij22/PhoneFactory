using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class CoffeeMachine : Machine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Coffee machine";
        public override string FormattedState => Current.Name;
        public override string Information => $"";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedSending { get; set; }
        public CoffeeMachine()
        {
            //Regular loop
            MyStates.Add(new State()
            {
                This = StateNames.A,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "A.",
                GetNext = () =>
                {
                    return StateNames.B;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.B,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "B.",
                GetNext = () =>
                {
                    if (!Factory.IsTurnedOn)
                        return StateNames.C;
                    return StateNames.A;
                }
            });

            MyStates.Add(new State()
            {
                This = StateNames.C,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "C.",
                GetNext = () =>
                {
                    if (Factory.IsTurnedOn)
                        return StateNames.A;
                    return StateNames.C;
                }
            });


            //Set first state
            Current = MyStates.First(state => state.This == StateNames.A);
        }
    }
}
