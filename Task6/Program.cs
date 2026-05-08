namespace Task6;

public static class RaceConditionExample
{
    private static int _counter = 0;
    private static readonly Lock Locker = new Lock(); 

    /*
     * В примере параллельно запускается множество потоков. Каждый поток в итерации берет значение счетчика,
     * увеличивает его на один и перезаписывает полученным значением переменную.
     * В результате работы программы ожидается,
     * что итоговое значение переменной будет равно количеству итераций в потоке, умноженному на количество потоков.
     * В таком варианте ожидается, что увелечение счетчика будет идти последовательно,
     * то есть каждое новое значение счётчика должно рассчитываться относительно предыдущего увеличенного значения.
     * Когда потоки работают параллельно, такого не происходит. Поток получает значение переменной,
     * но пока он дойдет до момента перезаписи переменной, это исходное значение может уже поменяться другими потоками.
     * Следовательно нужно производить описанную последовательность действий внутри блока lock,
     * чтобы обеспечить последовательный порядок получения-изменения-записи.
     */
    public static void Start()
    {
        const int numberOfThreads = 10;
        var threads = new Thread[numberOfThreads];

        for (var i = 0; i < numberOfThreads; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (var j = 0; j < 100_000; j++)
                {
                    lock (Locker)
                    {
                        _counter++;
                    }
                }
            });
            threads[i].Start();
        }

        for (var i = 0; i < numberOfThreads; i++)
        {
            threads[i].Join();
        }
        
        Console.WriteLine("Final counter value: " + _counter);
    }
}

public static class DeadlockExample
{
    private static readonly Lock Locker1 = new Lock();
    private static readonly Lock Locker2 = new Lock();

    /*
     * В данном примере получается, что thread1 блокирует Locker1, thread2 блокирует Locker2,
     * thread1 ждёт Locker2, а thread2 ждёт Locker1. То есть потоки заняли ресурсы таким образом,
     * что им не хватает ресурсов, чтобы завершить свою работу,
     * и при этом они не дают другим потокам завершить свою работу.
     * Чтобы предотвратить такие ситуации, нужно упорядочить доступ к ресурсам.
     * Возможно программа допускает, чтобы lock выполнялись не вложено, а на одном уровне.
     * Можно сразу заблокировать все необходимые ресурсы.
     * Либо осуществлять lock в определенном порядке - например, всегда Locker1 затем Locker2.
     * При таком подходе поток А может ожидать поток Б, но потоку Б уже точно не понадобятся ресурсы, нужные потоку А.
     * Ниже применен данный подход.
     */
    public static void Start()
    {
        var thread1 = new Thread(() =>
        {
            lock (Locker1)
            {
                Console.WriteLine("Thread 1 acquired Locker1");
                Thread.Sleep(50);
                lock (Locker2)
                {
                    Console.WriteLine("Thread 1 acquired Locker2");
                }
            }
        });
        var thread2 = new Thread(() =>
        {
            lock (Locker1)
            {
                Console.WriteLine("Thread 2 acquired Locker2");
                Thread.Sleep(50);
                lock (Locker2)
                {
                    Console.WriteLine("Thread 2 acquired Locker1");
                }
            }
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Finished");
    }
}

class Program
{
    static void Main()
    {
        RaceConditionExample.Start();
        DeadlockExample.Start();
    }
}