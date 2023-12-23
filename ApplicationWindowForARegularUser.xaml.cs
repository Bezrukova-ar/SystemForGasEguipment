using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZXing;

namespace SystemForGasEguipment
{
    /// <summary>
    /// Логика взаимодействия для ApplicationWindowForARegularUser.xaml
    /// </summary>
    public partial class ApplicationWindowForARegularUser : Window
    {
        public ApplicationWindowForARegularUser()
        {
            InitializeComponent();
        }


        //подать заявку на расширение прав доступа
        private void ApplyForExtensionOfAccessRights_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("UpdateVerificationRequest", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", LoginWindow.userID);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                MessageBox.Show("Ваша заявка отправлена на рассмотрение");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка, если это ваш первых вход, выйдите и зайдите");
            }
        }
        //получить информацию по qr-коду
        private void GetInformationOnAnImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BitmapSource bitmapSource = (BitmapSource)imageControl.Source;

            // Проверяем, что изображение не пустое
            if (bitmapSource != null)
            {
                // Преобразуем изображение в Bitmap
                var bitmap = BitmapFromSource(bitmapSource);

                var barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(bitmap);

                if (result != null)
                {
                    string queryString = "SELECT * FROM EguipmentItems Where eguipmentID = '" + result.ToString() + "'";

                    using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string message = $"Название: {reader.GetString(1)}" +
                               $"\nМодель: {reader.GetString(2)}\nСерийный номер: {reader.GetInt32(3)}"+
                               $"\nДата изготовления: {reader.GetDateTime(4)}\nГарантийный период: {reader.GetString(5)}"+
                               $"\nДавление газа: {reader.GetString(6)}\nТип газа: {reader.GetString(7)}" +
                               $"\nМатериал изготовления: {reader.GetString(8)}\nРазмер: {reader.GetString(9)}";
                                MessageBox.Show(message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не найдено.");
                        }
                        reader.Close();
                    }
                }
                else
                {
                    MessageBox.Show("QR-код не распознан", "Error");
                }
            }
            else
            {
                MessageBox.Show("Пустое изображение", "Error");
            }
        }
        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (var outStream = new System.IO.MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }
        //кнопка загрузки изображения
        private void UploadImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Открывает диалоговое окно проводника для выбора изображения
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Изображения|*.jpg;*.png;*.gif|Все файлы|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Загружает выбранное изображение в Image
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                imageControl.Source = bitmap;
            }
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    // Загружаем первый файл, если их несколько
                    BitmapImage bitmap = new BitmapImage(new Uri(files[0]));
                    imageControl.Source = bitmap;
                }
            }

        }

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            // Предотвращает стандартное действие при перетаскивании
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }
}
