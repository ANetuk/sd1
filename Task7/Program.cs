namespace Task7;

static class CurrentDateLogger
{
    public static void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        Console.WriteLine($"{timestamp}: {message}");
    }
}

static class SemaphoreExample
{
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(2);

    private static void WaitAndLog(int timeoutMs, string message)
    {
        Semaphore.Wait();
        try
        {
            Thread.Sleep(timeoutMs);
            CurrentDateLogger.Log(message);
        }
        finally
        {
            Semaphore.Release();
        }
    } 

    public static void Start()
    {
        CurrentDateLogger.Log("SemaphoreExample has starter");
        var threads = new Thread[]
        {
            new(() => WaitAndLog(500, "Thread 1 message")),
            new(() => WaitAndLog(100, "Thread 2 message")),
            new(() => WaitAndLog(100, "Thread 3 message")),
        };
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Start();
        }
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
        CurrentDateLogger.Log("SemaphoreExample has finished");
    }
}

static class ReaderWriterLockExample
{
    private static readonly List<string> Books = new List<string>();
    private static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

    private static void WriteBookWithTimeout(int timeout, string book)
    {
        Lock.EnterWriteLock();
        try
        {
            Thread.Sleep(timeout);
            Books.Add(book);
            CurrentDateLogger.Log($"The book has written: {book}");
        }
        finally
        {
            Lock.ExitWriteLock();
        }
    }

    private static void PrintBooksWithTimeout(int timeout)
    {
        Lock.EnterReadLock();
        try
        {
            Thread.Sleep(timeout);
            var booksString = string.Join(", ", Books);
            CurrentDateLogger.Log($"Current books: {booksString}");
        }
        finally
        {
            Lock.ExitReadLock();
        }
    }

    public static void Start()
    {
        CurrentDateLogger.Log("ReaderWriterLockExample has starter");
        var threads = new Thread[]
        {
            new(() => WriteBookWithTimeout(300, "The Lord of the Rings")),
            new(() => WriteBookWithTimeout(300, "Pride and Prejudice")),
            new(() => WriteBookWithTimeout(300, "His Dark Materials")), // В итоговом списке будет 3 книги
            new(() => PrintBooksWithTimeout(100)), // Ждёт записи
            new(() => PrintBooksWithTimeout(600)), // Чтение происходит одновременно
            new(() => PrintBooksWithTimeout(600)),
            new(() => PrintBooksWithTimeout(600))
        };
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Start();
        }

        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
        CurrentDateLogger.Log("ReaderWriterLockExample has finished");
    }
}

static class BarrierExample
{
    public static void Start()
    {
        CurrentDateLogger.Log("BarrierExample has starter");
        var barrier = new Barrier(2);
        var threads = new Thread[]
        {
            new (() => {
                Thread.Sleep(100);
                CurrentDateLogger.Log("Thread 1 has came to barrier");
                barrier.SignalAndWait();
                CurrentDateLogger.Log("Thread 1 has finished");
            }),
            new (() =>
            {
                Thread.Sleep(500);
                CurrentDateLogger.Log("Thread 2 has came to barrier");
                barrier.SignalAndWait();
                CurrentDateLogger.Log("Thread 2 has finished");
            })
        };
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Start();
        }
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
        CurrentDateLogger.Log("BarrierExample has finished");
    }
}

static class InterlockedExample
{
    private static int _count = 0;

    public static void Start()
    {
        CurrentDateLogger.Log("InterlockedExample has starter");
        const int threadsCount = 10;
        var threads = new Thread[threadsCount];
        for (var i = 0; i < threadsCount; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (var j = 0; j < 100_000; j++)
                {
                    Interlocked.Increment(ref _count);
                }
            });
            threads[i].Start();
        }
        for (var i = 0; i < threadsCount; i++)
        {
            threads[i].Join();
        }
        CurrentDateLogger.Log($"count = {_count}");
        CurrentDateLogger.Log("InterlockedExample has finished");
    }
}

static class MonitorExample
{
    public static void Start()
    {
        CurrentDateLogger.Log("MonitorExample has started");
        var locker = new object();
        var threads = new Thread[]
        {
            new(() =>
            {
                if (Monitor.TryEnter(locker, 2000))
                {
                    CurrentDateLogger.Log("Thread 1 has started");
                    Thread.Sleep(1000);
                    CurrentDateLogger.Log("Thread 1 has finished");
                }
                else
                {
                    CurrentDateLogger.Log("Thread 1 start timeout");
                }
            }),
            new(() =>
            {
                if (Monitor.TryEnter(locker, 2000))
                {
                    CurrentDateLogger.Log("Thread 2 has started");
                    Thread.Sleep(1000);
                    CurrentDateLogger.Log("Thread 2 has finished");
                }
                else
                {
                    CurrentDateLogger.Log("Thread 2 start timeout");
                }
            })
        };
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Start();
        }
        for (var i = 0; i < threads.Length; i++)
        {
            threads[i].Join();
        }
        CurrentDateLogger.Log("MonitorExample has finished");
    }
}

class Program
{
    public static void Main()
    {
        SemaphoreExample.Start();
        ReaderWriterLockExample.Start();
        BarrierExample.Start();
        InterlockedExample.Start();
        MonitorExample.Start();
    }
}
