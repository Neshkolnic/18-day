// Класс для лошади
using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class Horse
{
    public string Name { get; set; }
}

// Класс для пользователя

public class User
{
    public string Username { get; set; }
}

// Класс для ставки
public class Bet
{
    public User User { get; set; }
    public Horse Horse { get; set; }
    public decimal Amount { get; set; }
}

// Класс для забега
public class Race
{
    public List<Horse> Horses { get; set; }
}

// Класс для работы с базой данных
public class Database
{
    public List<User> Users { get; set; }
    public List<Horse> Horses { get; set; }
    public List<Bet> Bets { get; set; }

    public Database()
    {
        Users = new List<User>();
        Horses = new List<Horse>();
        Bets = new List<Bet>();
    }

    public void AddUser(string username)
    {
        Users.Add(new User { Username = username });
    }

    public void AddHorse(string name)
    {
        Horses.Add(new Horse { Name = name });
    }

    public void PlaceBet(User user, Horse horse, decimal amount)
    {
        Bets.Add(new Bet { User = user, Horse = horse, Amount = amount });
    }
}

// Класс для системы ставок
public class BettingSystem
{
    private readonly Database _database;

    public BettingSystem(Database database)
    {
        _database = database;
    }

    public void AddUser(string username)
    {
        _database.AddUser(username);
    }

    public void AddHorse(string name)
    {
        _database.AddHorse(name);
    }

    public void PlaceBet(User user, Horse horse, decimal amount)
    {
        _database.PlaceBet(user, horse, amount);
    }

    public void GenerateResults()
    {
        Random random = new Random();
        int winnerIndex = random.Next(_database.Horses.Count); // Выбираем случайную лошадь как победителя
        Horse winner = _database.Horses[winnerIndex];
        MessageBox.Show($"Победила лошадь: {winner.Name}", "Результаты забега");
    }


    public void CalculateWinnings()
    {
        foreach (var bet in _database.Bets)
        {
            if (bet.Horse.Name == _database.Horses[0].Name) // Проверяем, выиграла ли лошадь, на которую была сделана ставка
            {
                decimal winnings = bet.Amount * 2; // Удваиваем ставку для победителей
                MessageBox.Show($"Выигрыш пользователя {bet.User.Username}: {winnings}", "Выигрыш");
            }
            else
            {
                MessageBox.Show($"Пользователь {bet.User.Username} проиграл свою ставку", "Проигрыш");
            }
        }
    }

}
public static class GlobalVariables
{
    // Глобальная переменная для хранения логина авторизованного пользователя
    public static string LoggedInUser { get; set; }
    public static string conn{ get; set; }
    
    
}


