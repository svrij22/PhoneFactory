using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;

namespace PhoneFactory.Machines
{
    public class CoffeeMachine : FSMachine
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
        public bool KoffieZetApparaatStaatAan { get; set; }
        public bool KopjeStaatOnderMachine { get; set; }
        public CoffeeMachine()
        {
            //Regular loop
            States.Add(new State(MachineState.StaatAan, "Koffiezet apparaat staat uit", () =>
            {
                if (!KoffieZetApparaatStaatAan)
                    return MachineState.StaatUit;

                if (KopjeStaatOnderMachine)
                    return MachineState.ZetKoffie;

                return MachineState.StaatUit;
            }));

            States.Add(new State(MachineState.ZetKoffie, "Apparaat is koffie aan het zetten...", () =>
            {

                if (!KoffieZetApparaatStaatAan)
                    return MachineState.StaatUit;

                return MachineState.VerwijderUwBeker;
            }));

            States.Add(new State(MachineState.VerwijderUwBeker, "Verwijder uw beker...", () =>
            {

                if (KopjeStaatOnderMachine)
                    return MachineState.VerwijderUwBeker;

                if (!KoffieZetApparaatStaatAan)
                    return MachineState.StaatUit;

                return MachineState.StaatAan;
            }));

            States.Add(new State(MachineState.StaatUit, "Koffiezet apparaat staat uit", () =>
            {
                if (KoffieZetApparaatStaatAan)
                    return MachineState.StaatAan;
                return MachineState.StaatUit;
            }));

            //Set first state
            Current = States.First(state => state.Identifier == MachineState.StaatAan);
        }
    }
}
