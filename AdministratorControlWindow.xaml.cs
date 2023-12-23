using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SystemForGasEguipment
{
    /// <summary>
    /// Логика взаимодействия для AdministratorControlWindow.xaml
    /// </summary>
    public partial class AdministratorControlWindow : Window
    {
        public AdministratorControlWindow()
        {
            InitializeComponent();
            UploadingDataToExpandRights();
            UploadingDataToDemote();
        }
        public static int valueColumnForEmpowerment;
        public static int valueColumnForDemote;

        //Метод для загрузки данных о пользователях, которые хотят расширить права
        public void UploadingDataToExpandRights()
        {
            string query = "SELECT ID, username FROM Users WHERE roleID = 1 AND verificationRequest = 1";

            using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                DataView dataView = new DataView(dataTable);
                dataGridEmpowerment.ItemsSource = dataView;
                adapter.Dispose();
                connection.Close();
            }
        }
        //Метод для загрузки пользователей, которых можно разжаловать
        public void UploadingDataToDemote()
        {
            string query = "SELECT ID, username FROM Users WHERE roleID = 3";

            using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                DataView dataView = new DataView(dataTable);
                dataGridDemote.ItemsSource = dataView;
                adapter.Dispose();
                connection.Close();
            }
        }
        
        // для получения значения строки из таблицы с заявками для расширения должности
        private void dataGridEmpowerment_MouseClick(object sender, SelectedCellsChangedEventArgs e)
        {
            // Получаем выделенную строку
            DataRowView rowView = dataGridEmpowerment.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Получаем значения ячеек строки
                valueColumnForEmpowerment = (int)rowView.Row["ID"];
            }
        }
        // для получения значения строки из таблицы c пользователями на разжалование
        private void dataGridDemote_MouseClick(object sender, SelectedCellsChangedEventArgs e)
        {
            // Получаем выделенную строку
            DataRowView rowView = dataGridDemote.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Получаем значения ячеек строки
                valueColumnForDemote = (int)rowView.Row["ID"];
                MessageBox.Show(valueColumnForDemote.ToString());
            }
        }
        private void buttonEmpowerment_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateRoleEmpowerment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userID", valueColumnForEmpowerment);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                UploadingDataToExpandRights();
                UploadingDataToDemote();
            }
            catch
            {
                MessageBox.Show("Сначала выберите человека, которому следует расширить возможности");
            }

        }

        private void buttonDemote_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateRoleDemote", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userID", valueColumnForDemote);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                UploadingDataToDemote();
            }
            catch
            {
                MessageBox.Show("Сначала выберите человека, которого хотите разжаловать");
            }
        }
    }
}
