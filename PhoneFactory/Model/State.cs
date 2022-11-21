using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFactory.Machines
{
    public enum MachineState
    {
        //Metal encasing machine
        PoweredOff,
        PoweredOn,
        WarmingUpOven,
        WarmedUp,
        MeltingMetal,
        MoldingMetal,
        CoolingCases,
        SendToBelt,
        CoolingDown,

        Sending,
        PreparingMicrochips,
        EmbeddingCases,
        AttachWiring,
        ZetKoffie,
        StaatAan,
        StaatUit,
        VerwijderUwBeker,
        Resuming,
        PackagingPhone,
        WrappingProducts,
        MarkingPhoneGUID,
    }
    public class State
    {

        public MachineState Identifier;
        public Func<MachineState> GetNext { get; set; }

        public string Name;

        public TimeSpan TimeSpan = TimeSpan.FromMilliseconds(400);
        public State() { }
        public State(MachineState identifier, string name, Func<MachineState> getNext)
        {
            Identifier = identifier;
            GetNext = getNext;
            Name = name;
        }
    }
}
