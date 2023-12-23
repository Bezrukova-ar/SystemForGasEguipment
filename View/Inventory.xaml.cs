using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

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
        }
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
    }
}