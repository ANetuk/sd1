namespace Task2;

class BankAccount
{
    private double _balance;

    public BankAccount(double balance)
    {
        _balance = balance;
    }

    public void Deposit(double amount)
    {
        _balance += amount;
    }

    public void Withdraw(double amount)
    {
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
        Console.WriteLine(bankAccount.GetBalance()); // Expected output: 0
        
        bankAccount.Deposit(1000);
        Console.WriteLine(bankAccount.GetBalance()); // Expected output: 1000
        
        bankAccount.Withdraw(500);
        Console.WriteLine(bankAccount.GetBalance()); // Expected output: 500
        
        bankAccount.Withdraw(100);
        Console.WriteLine(bankAccount.GetBalance()); // Expected output: 400

        bankAccount.Deposit(200);
        Console.WriteLine(bankAccount.GetBalance()); // Expected output: 600
    }
}
