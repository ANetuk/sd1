namespace Task3;

class BankAccount
{
    private double _balance;

    public BankAccount(double balance)
    {
        _balance = balance;
    }

    public void Deposit(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма для депозита не может быть 0 или отрицательным числом");
            return;
        }

        _balance += amount;
    }

    public void Withdraw(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Сумма для снятия не может быть 0 или отрицательным числом");
            return;
        }
        if (amount > _balance)
        {
            Console.WriteLine("Сумма для снятия не может быть больше чем баланс");
            return;
        }

        _balance -= amount;
    }

    public double GetBalance()
    {
        return _balance;
    }
}

class Program
{
    public static void Main()
    {
        var bankAccount = new BankAccount(0);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 0
        
        bankAccount.Deposit(1000);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 1000
        
        bankAccount.Withdraw(500);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 500
        
        bankAccount.Withdraw(100);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 400

        bankAccount.Deposit(200);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 600
        
        bankAccount.Deposit(-100);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 600, так как указана некорректная сумма
        
        bankAccount.Withdraw(-100);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 600, так как указана некорректная сумма
        
        bankAccount.Withdraw(bankAccount.GetBalance() + 1);
        Console.WriteLine(bankAccount.GetBalance()); // Ожидается: 600, так как указана некорректная сумма
    }
}
