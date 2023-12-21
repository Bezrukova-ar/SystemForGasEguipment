using System.Windows;
using System.Windows.Input;

namespace SystemForGasEguipment
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void OpenTheApplication(object sender, RoutedEventArgs e)
        {
            // тут ддолжна быть обработка, в зависимости от роли пользователя открывается соответствубщее окно
            //пока только окно для гостя
            /* ApplicationWindowForARegularUser newWindow = new ApplicationWindowForARegularUser();
             newWindow.Show();*/

            /* AdministratorControlWindow newWindow = new AdministratorControlWindow();
             newWindow.Show();*/

            AccountingAndMarkingOfEquipment newWindow = new AccountingAndMarkingOfEquipment();
            newWindow.Show();

            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
