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
        }
        public static int col1Value;
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
        // для получения значения строки
        private void dataGridEmpowerment_MouseClick(object sender, SelectedCellsChangedEventArgs e)
        {
            // Получаем выделенную строку
            DataRowView rowView = dataGridEmpowerment.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Получаем значения ячеек строки
                col1Value =(int)rowView.Row["ID"];
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
                        command.Parameters.AddWithValue("@userID", col1Value);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                UploadingDataToExpandRights();
            }
            catch
            {
                MessageBox.Show("Сначала выберите человека, которому следует расширить возможности");
            }

        }
    }
}
