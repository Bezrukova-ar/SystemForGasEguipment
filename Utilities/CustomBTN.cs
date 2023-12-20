using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SystemForGasEguipment.Utilities
{
    public class CustomBTN : RadioButton
    {
        static CustomBTN()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomBTN), new FrameworkPropertyMetadata(typeof(CustomBTN)));
        }
    }
}
