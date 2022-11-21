using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneFactory.Machines
{
    public class Factory
    {
        public ObservableCollection<FSMachine> Machines { get; set; }
        public static bool IsTurnedOn { get; set; } = false;
        public void AddMachines()
        {
            Machines = new();

            var machine1 = new MetalEncasingMachine();
            machine1.Start();
            Machines.Add(machine1);

            var belt1 = new ConveyorBelt();
            belt1.Start();
            Machines.Add(belt1);

            var machine2 = new MicrochipMachine();
            machine2.Start();
            Machines.Add(machine2);

            var belt2 = new ConveyorBelt();
            belt2.Start();
            Machines.Add(belt2);

            var pMach = new PackagingMachine();
            pMach.Start();
            Machines.Add(pMach);

            var belt3 = new ConveyorBelt();
            belt3.Start();
            Machines.Add(belt3);

            var depot = new PackageDepot();
            depot.Start();
            Machines.Add(depot);

            var cfM = new CoffeeMachine();
            cfM.Start();
            Machines.Add(cfM);

            machine1.NextMachine = belt1;
            belt1.NextMachine = machine2;
            machine2.NextMachine = belt2;
            belt2.NextMachine = pMach;
            pMach.NextMachine = belt3;
            belt3.NextMachine = depot;

        }
    }
}
