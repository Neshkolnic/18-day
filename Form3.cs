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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.horseRacingTotalizatorDataSet);

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "horseRacingTotalizatorDataSet.History". При необходимости она может быть перемещена или удалена.
            this.historyTableAdapter.Fill(this.horseRacingTotalizatorDataSet.History);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "horseRacingTotalizatorDataSet.Users". При необходимости она может быть перемещена или удалена.
            this.usersTableAdapter.Fill(this.horseRacingTotalizatorDataSet.Users);

        }
        private void ClearBetsTable()
        {
            string connectionString = GlobalVariables.conn;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Bets";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show($"Удалено {rowsAffected} записей из таблицы ставок.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении записей из таблицы ставок: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(GlobalVariables.conn))
            {


                string query = "SELECT TOP 1 HorseID FROM Bets ORDER BY NEWID()"; // Выбираем случайную лошадь
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    int winningHorseID = (int)command.ExecuteScalar();

                    // Добавляем записи в таблицу истории ставок
                    query = "INSERT INTO History (BetID, UserID, HorseID, Amount, BetType, Outcome) " +
                            "SELECT BetID, UserID, HorseID, " +
                            "CASE WHEN HorseID = @WinningHorseID THEN Amount * 3.75 ELSE 0 END, " +
                            "BetType, " +
                            "CASE WHEN HorseID = @WinningHorseID THEN 'Win' ELSE 'Lose' END " +
                            "FROM Bets";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@WinningHorseID", winningHorseID);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Ставки обработаны и добавлены в историю.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обработать ставки.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обработки ставок и добавления в историю: " + ex.Message);
                }
                ClearBetsTable();
                historyDataGridView.Update();
            }
        }
    }
}
