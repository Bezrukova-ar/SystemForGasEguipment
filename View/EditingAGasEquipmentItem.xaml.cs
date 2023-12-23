using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace SystemForGasEguipment.View
{
    /// <summary>
    /// Логика взаимодействия для EditingAGasEquipmentItem.xaml
    /// </summary>
    public partial class EditingAGasEquipmentItem : UserControl
    {
        public EditingAGasEquipmentItem()
        {
            InitializeComponent();
            UploadingDataGasEquipment();
        }
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
        //Обновление данных
        private void dataGridGasEquipment_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if(e.Column.DisplayIndex!=0)
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    var editedItem = e.Row.Item as DataRowView;
                    var columnName = e.Column.Header.ToString(); // Получаем имя столбца, который был отредактирован
                    var newValue = ((TextBox)e.EditingElement).Text; // Получаем отредактированное значение
                    using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE EguipmentItems SET " + columnName + " = '" + newValue + "' " + "WHERE eguipmentID = " + editedItem["eguipmentID"];
                        SqlCommand command = new SqlCommand(updateQuery, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }          
            }
            UploadingDataGasEquipment();
        }
    }
}
