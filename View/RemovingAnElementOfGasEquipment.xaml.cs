using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SystemForGasEguipment.View
{
    /// <summary>
    /// Логика взаимодействия для RemovingAnElementOfGasEquipment.xaml
    /// </summary>
    public partial class RemovingAnElementOfGasEquipment : UserControl
    {
        public RemovingAnElementOfGasEquipment()
        {
            InitializeComponent();
            UploadingDataGasEquipment();
        }
        public static int valueColumn;
        //Метод для загрузки данных 
        public void UploadingDataGasEquipment()
        {
            string query = "SELECT*FROM EguipmentItems";

            using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                DataView dataView = new DataView(dataTable);
                dataGridGasEquipment.ItemsSource = dataView;
                adapter.Dispose();
                connection.Close();
            }
        }
        //Динамический поиск с подгрузкой
        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataView dataView = dataGridGasEquipment.ItemsSource as DataView;

            if (dataView != null)
            {
                if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
                {
                    dataView.RowFilter = "";
                }
                else
                {
                    string filterExpression = $"name LIKE '%{textBoxSearch.Text}%'";
                    dataView.RowFilter = filterExpression;
                }
            }
        }
        //Получение значения выделеннрой строки
        private void dataGridGasEquipment_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            // Получаем выделенную строку
            DataRowView rowView = dataGridGasEquipment.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Получаем значения ячеек строки
                valueColumn = (int)rowView.Row["eguipmentID"];
            }
        }
        //Удаление
        private void buttonDeleteDasEquioment_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("DeleteGasEquipment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@eguipmentID", valueColumn);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                UploadingDataGasEquipment();
            }
            catch
            {
                MessageBox.Show("Сначала выберите  какой элемент газового оборудования хотите удалить");
            }
        }
    }
}
