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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Получаем введенные пользователем данные
            string username = textBox1.Text;
            string password = textBox2.Text;
            GlobalVariables.conn  = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HorseRacingTotalizator;Integrated Security=True";
            GlobalVariables.LoggedInUser = textBox1.Text;

            // Создаем подключение к базе данных
            using (SqlConnection connection = new SqlConnection(GlobalVariables.conn))
            {
                // Создаем команду SQL для выборки пользователя из базы данных
                string query = "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string role = reader.GetString(0); // Получаем роль пользователя из базы данных
                                                           // Проверяем роль пользователя
                        if (role == "Admin")
                        {
                            // Если пользователь - администратор, переходим на форму 3
                            Form3 adminForm = new Form3();
                            adminForm.Show();
                            this.Hide(); // Скрываем текущую форму
                        }
                        else if (role == "User")
                        {
                            // Если пользователь - обычный пользователь, переходим на форму 2
                            Form2 userForm = new Form2();
                            userForm.Show();
                           
                            this.Hide(); // Скрываем текущую форму
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверные логин или пароль");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
}
