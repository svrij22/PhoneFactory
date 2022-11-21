using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFactory.Machines
{
    public enum StateNames
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
        B,
        A,
        C,
    }
    public class State
    {
        public StateNames This;

        public string Name;

        public TimeSpan TimeSpan;
        public Func<StateNames> GetNext { get; set; }
    }
}
