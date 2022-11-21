using PhoneFactory.Machines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneFactory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FactoryWindow : Window
    {
        public Factory Factory { get; }
        public FactoryWindow()
        {
            InitializeComponent();

            Factory = new Factory();
            DataContext = Factory;

            Factory.AddMachines();
        }

        private void TurnOn_Click(object sender, RoutedEventArgs e)
        {
            Factory.IsTurnedOn = true;
        }

        private void TurnOff_Click(object sender, RoutedEventArgs e)
        {
            Factory.IsTurnedOn = false;
        }

        private void AddPhones_Click(object sender, RoutedEventArgs e)
        {
            Factory.Machines[0].QueuedItems += 10;
        }
    }
}
