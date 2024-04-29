using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _18_day
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "horseRacingTotalizatorDataSet.Bets". При необходимости она может быть перемещена или удалена.
            LoadBetsIntoListBox();
            LoadHorsesIntoComboBox();
            this.Text = GlobalVariables.LoggedInUser;
        }

        private void LoadBetsIntoListBox()
        {
            // Создаем подключение к базе данных
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HorseRacingTotalizator;Connect Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Создаем команду SQL для выборки всех ставок из таблицы Bets
                string query = "SELECT UserID, HorseID, Amount FROM Bets";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Очищаем ListBox1 перед загрузкой новых данных
                    listBox1.Items.Clear();

                    // Добавляем каждую ставку в ListBox1
                    while (reader.Read())
                    {
                        // Формируем строку для отображения в ListBox1
                        string betInfo = $"Пользователь: {reader.GetInt32(0)}, Лошадь: {reader.GetInt32(1)}, Сумма ставки: {reader.GetDecimal(2)}";
                        listBox1.Items.Add(betInfo);
                    }

                    // Закрываем DataReader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки ставок из базы данных: " + ex.Message);
                }
            }
        }

        public void LoadHorsesIntoComboBox()
        {
            // Создаем подключение к базе данных
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HorseRacingTotalizator;Connect Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Создаем команду SQL для выборки всех лошадей из таблицы Horses
                string query = "SELECT Name FROM Horses";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Очищаем ComboBox перед загрузкой новых данных
                    comboBox1.Items.Clear();

                    // Добавляем каждого лошадь в ComboBox
                    while (reader.Read())
                    {
                        string horseName = reader.GetString(0);
                        comboBox1.Items.Add(horseName);
                    }

                    // Закрываем DataReader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки лошадей из базы данных: " + ex.Message);
                }
            }
        }
        private void addStavki(Label lb)
        {
            textBox1.Text = (int.Parse(lb.Text) + int.Parse(textBox1.Text)).ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            addStavki(label3);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            addStavki(label5);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            addStavki(label4);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            addStavki(label6);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
           
            // Получаем имя текущего пользователя
            string username = GlobalVariables.LoggedInUser;

            // Получаем сумму ставки
            decimal amount;
            if (!decimal.TryParse(textBox1.Text, out amount))
            {
                MessageBox.Show("Введите корректную сумму ставки.");
                return;
            }

            // Получаем выбранное значение из ComboBox для выбранной лошади
            string horseName = comboBox1.SelectedItem as string;
            if (string.IsNullOrEmpty(horseName))
            {
                MessageBox.Show("Выберите лошадь для ставки.");
                return;
            }

            // Получаем ID лошади из базы данных по ее имени
            int horseID;

            using (SqlConnection connection = new SqlConnection(GlobalVariables.conn))
            {
                string query = "SELECT HorseID FROM Horses WHERE Name = @Name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", horseName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out horseID))
                    {
                        // Получаем ID пользователя из базы данных по его логину
                        int userID;
                        query = "SELECT UserID FROM Users WHERE Username = @Username";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Username", username);
                        result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out userID))
                        {
                            // Добавляем новую ставку в базу данных
                            query = "INSERT INTO Bets (UserID, HorseID, Amount, BetType) VALUES (@UserID, @HorseID, @Amount, @BetType)";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@UserID", userID);
                            command.Parameters.AddWithValue("@HorseID", horseID);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@BetType", "base"); // Здесь нужно указать тип ставки

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Ставка успешно добавлена.");
                            }
                            else
                            {
                                MessageBox.Show("Не удалось добавить ставку.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Пользователь с таким логином не найден.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не удалось определить ID лошади.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления ставки в базу данных: " + ex.Message);
                }
            }
            LoadBetsIntoListBox();
        } 

        private void betsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
        }

    }
}
