using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Linq;

namespace SystemForGasEguipment
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        static public string connectionString = "Data Source=DESKTOP-8LRHPT1;Initial Catalog=GasEquipment;Integrated Security=True";
        public LoginWindow()
        {
            InitializeComponent();
        }

        //открывание формы
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
        
        //открытие формы после регистрации   
        private void OpenTheApplicationAndRegistration(object sender, RoutedEventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password1 = textBoxPassword.Password.Trim();
            string password2 = textBoxRepeadPassword.Password.Trim();

            if (password1.Length != 0)
            {
                if (IsPasswordSecure(password1))
                {
                    // Проверка совпадения паролей
                    if (password1 != password2)
                    {
                        MessageBox.Show("Пароли не совпадают.");
                        return;
                    }
                    else
                    {

                        // Создание подключения к базе данных
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Создание команды для вызова хранимой процедуры
                            using (SqlCommand command = new SqlCommand("RegisterUser", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                // Добавление параметров
                                command.Parameters.AddWithValue("@username", username);
                                command.Parameters.AddWithValue("@password", password1);

                                // Выполнение команды
                                try
                                {
                                    command.ExecuteNonQuery();
                                    MessageBox.Show("Регистрация успешно завершена.");
                                    ApplicationWindowForARegularUser newWindow = new ApplicationWindowForARegularUser();
                                    newWindow.Show();
                                    this.Close();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show("Ошибка при регистрации: " + ex.Message);
                                }
                            }
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Пароль недостаточно надежный. Пароль должен содержать минимум 8 символов, хотя бы одну цифру, одну букву в верхнем регистре, одну букву в нижнем регистре и один специальный символ.");
                }
            }              
        }
        //Метод убирает лишнии символы из логина
        private void TextBoxUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из текстбокса
            string username = textBoxUsername.Text;

            // Проверяем, что количество символов не превышает 50
            if (username.Length > 50)
            {
                // Удаляем последний введенный символ
                textBoxUsername.Text = username.Substring(0, username.Length - 1);

                // Устанавливаем курсор в конец текста
                textBoxUsername.Select(textBoxUsername.Text.Length, 0);
            }

            // Проверяем наличие знаков препинания
            foreach (char c in username)
            {
                if (char.IsPunctuation(c))
                {
                    // Удаляем знак препинания из текста
                    textBoxUsername.Text = textBoxUsername.Text.Replace(c.ToString(), "");

                    // Устанавливаем курсор в конец текста
                    textBoxUsername.Select(textBoxUsername.Text.Length, 0);
                }
            }
        }
        //Метод проверки пароля на надежность
        private bool IsPasswordSecure(string password)
        {
            bool isSecure = true;

            // Проверяем, чтобы пароль был не меньше 8 символов
            if (password.Length < 8)
                isSecure = false;

            // Проверяем, чтобы пароль содержал хотя бы одну цифру
            if (!password.Any(char.IsDigit))
                isSecure = false;

            // Проверяем, чтобы пароль содержал хотя бы одну букву в верхнем регистре
            if (!password.Any(char.IsUpper))
                isSecure = false;

            // Проверяем, чтобы пароль содержал хотя бы одну букву в нижнем регистре
            if (!password.Any(char.IsLower))
                isSecure = false;

            // Проверяем, чтобы пароль содержал хотя бы один специальный символ
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                isSecure = false;

            return isSecure;
        }
    }
}
