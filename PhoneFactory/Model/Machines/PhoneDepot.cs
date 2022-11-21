using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class PhoneDepot : Machine
    {

        /// <summary>
        /// State
        /// </summary>
        /// 

        public override string Name { get; set; } = "Smartphone depot";
        public override string FormattedState => Current.Name;
        public override string Information => $"{QueuedItems}";


        /// <summary>
        /// Info
        /// </summary>
        public int StartedSending { get; set; }
        public PhoneDepot()
        {
            //Regular loop
            MyStates.Add(new State()
            {
                This = StateNames.PoweredOn,
                TimeSpan = TimeSpan.FromSeconds(1),
                Name = "Phone depot.",
                GetNext = () =>
                {
                    return StateNames.PoweredOn;
                }
            });

            //Set first state
            Current = MyStates.First(state => state.This == StateNames.PoweredOn);
        }
    }
}
