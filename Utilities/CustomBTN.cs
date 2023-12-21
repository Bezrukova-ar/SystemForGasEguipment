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
