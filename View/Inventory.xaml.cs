using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ZXing;
using ZXing.QrCode;

namespace SystemForGasEguipment.View
{
    /// <summary>
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class Inventory : UserControl
    {
        public Inventory()
        {
            InitializeComponent();
            UploadingDataGasEquipment();
        }
        public static int valueColumn;
        //кнопка для сохранения результата инвентаризации
        private void buttonInventory_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(LoginWindow.connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("InventoryGasEquipment", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Save to PDF
                            SaveDataTableToPDF(dt);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
        //Метод сохранения данных в пдф формате
        private void SaveDataTableToPDF(DataTable dt)
        {
            BaseFont baseFont = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); // Путь к файлу шрифта Arial
            Font font = new Font(baseFont, 12, Font.NORMAL);
            Document document = new Document();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF File|*.pdf";
            saveFileDialog1.Title = "Save PDF File";

            if (saveFileDialog1.ShowDialog() == true)
            {
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog1.FileName, FileMode.Create));
                document.Open();

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;

                foreach (DataColumn column in dt.Columns)
                {
                    table.AddCell(new Phrase(column.ColumnName, font));
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (object cell in row.ItemArray)
                    {
                        table.AddCell(new Phrase(cell.ToString(), font));
                    }
                }
                document.Add(table);
                document.Close();
            }
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
        //сгенирировать QR-код
        private void buttonQRcode_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250
                }
            };

            System.Drawing.Bitmap qrCode = writer.Write(valueColumn.ToString());

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files|*.png";
            saveFileDialog.Title = "Save QR Code";
            saveFileDialog.FileName = "qr_code.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                qrCode.Save(saveFileDialog.FileName);
                MessageBox.Show("QR-код успешно сохранен");
            }
        }
    
        // получить значение ячейки
        private void dataGridGasEquipment_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataRowView rowView = dataGridGasEquipment.SelectedItem as DataRowView;
            if (rowView != null)
            {
                // Получаем значения ячеек строки
                valueColumn = (int)rowView.Row["eguipmentID"];
            }
        }
    }
}