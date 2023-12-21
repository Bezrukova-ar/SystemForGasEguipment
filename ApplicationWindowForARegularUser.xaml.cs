using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        }
        //получить информацию по qr-коду
        private void GetInformationOnAnImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

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
