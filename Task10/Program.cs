namespace Task10;

class Program
{
    private const int Threads = 4;
    private const int Size = 1_000_000;
    private static readonly int[] Data = new int[Size];
    private static volatile int _sum = 0;
    
    public static void Main()
    {
        for (var i = 0; i < Size; i++)
        {
            Data[i] = Random.Shared.Next(100);
        }

        const int chunkSize = (Size + Threads - 1) / Threads;
        var threads = Enumerable
            .Range(0, Threads)
            .Select(i => new Thread(() =>
            {
                var skippedCount = i * chunkSize;
                var localSum = Data.Skip(skippedCount).Take(chunkSize).Sum();
                Interlocked.Add(ref _sum, localSum);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Sum of all elements: " + _sum);
    }
}