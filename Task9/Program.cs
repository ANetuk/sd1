namespace Task9;

class Program
{
    private static int _counter = 0;

    public static void Main()
    {
        ThreadStart task = () =>
        {
            for (var i = 0; i < 1000; i++)
            {
                Interlocked.Increment(ref _counter);
            }
        };

        var thread1 = new Thread(task);
        var thread2 = new Thread(task);
        
        thread1.Start();
        thread2.Start();

        try
        {
            thread1.Join();
            thread2.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Console.WriteLine("Counter: " + Volatile.Read(ref _counter));
    }
}
