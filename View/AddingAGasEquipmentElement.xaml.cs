using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SystemForGasEguipment.View
{
    /// <summary>
    /// Логика взаимодействия для AddingAGasEquipmentElement.xaml
    /// </summary>
    public partial class AddingAGasEquipmentElement : UserControl
    {
        public AddingAGasEquipmentElement()
        {
            InitializeComponent();
        }
        //Добавить данные в базу данных
        private void Button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string nameEquioment = nameEquipmentTextBox.Text;
            string model = modelTextBox.Text;
            string number = numberTextBox.Text;
            string date = dateTextBox.Text;
            string garant = garantTextBox.Text;
            string gasType = gasTypeTextBox.Text;
            string material = materialTextBox.Text;
            string pressure = pressuareTextBox.Text;
            string size = sizeTextBox.Text;
            if (nameEquioment.Length!=0&& model.Length != 0 && number.Length != 0 && date.Length != 0
                  && garant.Length != 0 && gasType.Length != 0 && material.Length != 0 && pressure.Length != 0 && size.Length != 0)
            {
                using (SqlConnection connection = new SqlConnection(LoginWindow.connectionString))
                {
                    using (SqlCommand command = new SqlCommand("AddGasEguipment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        // Добавление параметров
                        command.Parameters.AddWithValue("@name", nameEquioment);
                        command.Parameters.AddWithValue("@equipmentItemModel", model);
                        command.Parameters.AddWithValue("@serial_number", number);
                        command.Parameters.AddWithValue("@dateОfManufacture", date);
                        command.Parameters.AddWithValue("@guaranteePeriod", garant);
                        command.Parameters.AddWithValue("@gasPressure", gasType);
                        command.Parameters.AddWithValue("@gasType", material);
                        command.Parameters.AddWithValue("@materialOfManufacture", pressure);
                        command.Parameters.AddWithValue("@dimensions", size);


                        // Выполнение хранимой процедуры
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show("Элемент газового оборудования  успешно добавлен",
                          "Message", MessageBoxButton.OK,
                          MessageBoxImage.Information);
                        connection.Close();
                    }
                    nameEquipmentTextBox.Clear();
                    modelTextBox.Clear();
                    numberTextBox.Clear();
                    dateTextBox.Clear();
                    garantTextBox.Clear();
                    gasTypeTextBox.Clear();
                    materialTextBox.Clear();
                    pressuareTextBox.Clear();
                    sizeTextBox.Clear();
                }
            }
            else 
            {
                MessageBox.Show("Заполните все поля");
            }

        }

        //Ограничивает ввод и запрещает ввод любых сиволов кроме букв для поля с именем оборудования
        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nameEquipment = nameEquipmentTextBox.Text;
            // Проверяем, что количество символов не превышает 70
            if (nameEquipment.Length > 70)
            {
                nameEquipmentTextBox.Text = nameEquipment.Substring(0, nameEquipment.Length - 1);
                nameEquipmentTextBox.Select(nameEquipmentTextBox.Text.Length, 0);
            }
            foreach(char c in nameEquipment)
            {
                if (!Regex.IsMatch(nameEquipment, @"^[а-яА-Я\s]+$"))
                {
                    nameEquipmentTextBox.Text = nameEquipment.Substring(0, nameEquipment.Length - 1);
                    nameEquipmentTextBox.Select(nameEquipmentTextBox.Text.Length, 0);
                }
            }         
        }
        //Ограничивает ввод и разрешает вводить только английский буквы, тире и цифры
        private void modelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из текстбокса
            string model = modelTextBox.Text;
            // Проверяем, что количество символов не превышает 70
            if (model.Length > 70)
            {
                // Удаляем последний введенный символ
                modelTextBox.Text = model.Substring(0, model.Length - 1);
                // Устанавливаем курсор в конец текста
                modelTextBox.Select(modelTextBox.Text.Length, 0);
            }
            else
            {
                // Проверяем, что введенный текст соответствует требуемому формату
                string pattern = @"^[A-Za-z0-9\-]*$";
                bool isMatch = Regex.IsMatch(model, pattern, RegexOptions.IgnoreCase);

                if (!isMatch)
                {
                    // Удаляем недопустимые символы из текста
                    model = Regex.Replace(model, @"[^A-Za-z0-9\-]", string.Empty);
                    modelTextBox.Text = model;
                    // Устанавливаем курсор в конец текста
                    modelTextBox.Select(modelTextBox.Text.Length, 0);
                }
            }
        }
        //Ограничивает воод и разрешает только цифры
        private void numberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из текстбокса
            string number = numberTextBox.Text;

            // Проверяем, что количество символов не превышает 10
            if (number.Length > 10)
            {
                // Удаляем последний введенный символ
                numberTextBox.Text = number.Substring(0, number.Length - 1);

                // Устанавливаем курсор в конец текста
                numberTextBox.Select(numberTextBox.Text.Length, 0);
            }
            // Проверяем, что введенный символ является цифрой
            foreach (char c in number)
            {
                if (!Regex.IsMatch(number, "^[0-9]+$"))
                {
                    // Удаляем последний введенный символ
                    numberTextBox.Text = number.Substring(0, number.Length - 1);
                    // Устанавливаем курсор в конец текста
                    numberTextBox.Select(numberTextBox.Text.Length, 0);
                }
            }
        }
        //ограничение для ввода даты
        private void dateTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            {
                // Проверяем, что вводимый символ является числом или точкой
                if (!char.IsDigit(e.Text[0]) && e.Text[0] != '.')
                {
                    e.Handled = true; // Игнорируем ввод символа, если он не является числом или точкой
                    return;
                }

                // Получаем текущий текст из текстбокса
                string currentText = dateTextBox.Text;
                // Проверяем, что количество символов не превышает 10
                if (currentText.Length > 10)
                {
                    // Удаляем последний введенный символ
                    dateTextBox.Text = currentText.Substring(0, currentText.Length - 1);

                    // Устанавливаем курсор в конец текста
                    dateTextBox.Select(nameEquipmentTextBox.Text.Length, 0);
                }
                // Получаем позицию курсора
                int caretIndex = dateTextBox.CaretIndex;

                // Проверяем, можно ли добавить вводимый символ без нарушения формата "дд.мм.гггг"
                if (currentText.Contains(".") && (caretIndex != currentText.Length))
                {
                    e.Handled = true; // Игнорируем ввод символа, если в поле уже присутствует точка и курсор не стоит в конце строки
                    return;
                }

                // Проверяем полную строку с учетом вводимого символа
                string newText = currentText.Insert(caretIndex, e.Text);
                string[] parts = newText.Split('.');

                // Проверяем, что вводимая часть является числом и соответствует ожидаемому формату
                if (parts.Length > 0 && parts[0].Length > 2)
                {
                    e.Handled = true; // Игнорируем ввод символа, если вводимый день имеет более двух символов
                    return;
                }
                if (parts.Length > 1 && parts[1].Length > 2)
                {
                    e.Handled = true; // Игнорируем ввод символа, если вводимый месяц имеет более двух символов
                    return;
                }
                if (parts.Length > 2 && parts[2].Length > 4)
                {
                    e.Handled = true; // Игнорируем ввод символа, если вводимый год имеет более четырех символов
                    return;
                }
            }
        }
        //Убирает сиволы из срока гарантии
        private void garantTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из текстбокса
            string garant = garantTextBox.Text;

            // Проверяем, что количество символов не превышает 10
            if (garant.Length > 10)
            {
                // Удаляем последний введенный символ
                garantTextBox.Text = garant.Substring(0, garant.Length - 1);

                // Устанавливаем курсор в конец текста
                garantTextBox.Select(garantTextBox.Text.Length, 0);
            }

            // Проверяем наличие знаков препинания
            foreach (char c in garant)
            {
                if (char.IsPunctuation(c))
                {
                    // Удаляем знак препинания из текста
                    garantTextBox.Text = garantTextBox.Text.Replace(c.ToString(), "");

                    // Устанавливаем курсор в конец текста
                    garantTextBox.Select(garantTextBox.Text.Length, 0);
                }
            }
        }
        //Тип газа ограничение ввода
        private void gasTypeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получаем текст из текстбокса
            string gasType = gasTypeTextBox.Text;

            // Проверяем, что количество символов не превышает 50
            if (gasType.Length > 40)
            {
                // Удаляем последний введенный символ
                gasTypeTextBox.Text = gasType.Substring(0, gasType.Length - 1);

                // Устанавливаем курсор в конец текста
                gasTypeTextBox.Select(gasTypeTextBox.Text.Length, 0);
            }

            // Проверяем, что введенный символ является буквой
            foreach (char c in gasType)
            {
                if (!Regex.IsMatch(gasType, @"^[а-яА-Я\s]+$"))
                {
                    // Удаляем последний введенный символ
                    nameEquipmentTextBox.Text = gasType.Substring(0, gasType.Length - 1);
                    // Устанавливаем курсор в конец текста
                    nameEquipmentTextBox.Select(nameEquipmentTextBox.Text.Length, 0);
                }
            }
        }
        //Ограничение ввода материала элемента газового оборудования
        private void materialTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string material = materialTextBox.Text;

            // Проверяем, что количество символов не превышает 50
            if (material.Length > 40)
            {
                // Удаляем последний введенный символ
                materialTextBox.Text = material.Substring(0, material.Length - 1);

                // Устанавливаем курсор в конец текста
                materialTextBox.Select(materialTextBox.Text.Length, 0);
            }

            // Проверяем, что введенный символ является буквой
            foreach (char c in material)
            {
                if (!Regex.IsMatch(material, @"^[а-яА-Я\s]+$"))
                {
                    // Удаляем последний введенный символ
                    materialTextBox.Text = material.Substring(0, material.Length - 1);
                    // Устанавливаем курсор в конец текста
                    materialTextBox.Select(materialTextBox.Text.Length, 0);
                }
            }
        }
    }
}
