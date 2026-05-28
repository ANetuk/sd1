using System.Text.Json;

namespace Task16;

class Animal
{
    public void MakeGenericSound()
    {
        Console.WriteLine("Some generic animal sound");
    }

    public virtual void Feed()
    {
        Console.WriteLine("Animal feed");
    }
}

class Cat : Animal
{
    public void MakeSound()
    {
        Console.WriteLine("Meow");
    }

    public override void Feed()
    {
        Console.WriteLine("Cat feed");
    }

    public void Feed(int numberOfFeed)
    {
        for (var i = 0; i < numberOfFeed; i++)
        {
            Console.WriteLine("Cat feed");
        }
    }
}

public static class DeserializeExample
{
    public static void Deserialize()
    {
        try
        {
            var jsonString = "{\"name\":\"John\", \"age\":30}";
            var person = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            Console.WriteLine($"Name: {person.GetValueOrDefault("name")}");
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            var serializedString = JsonSerializer.Serialize(person, options);
            Console.WriteLine($"Pretty JSON: {serializedString}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

class Program
{
    static void Main()
    {
        Animal myCat = new Cat();

        myCat.MakeGenericSound();
        // 1. После переименования вызовется метод базового класса Animal.
        //    В данной ситуации формируется неочевидная связь между классами и клиентом,
        //    Поэтому легко допустить ситуацию, когда код для клиента отработает неожиданным образом.
        
        myCat.Feed();
        // myCat.Feed(3);
        // 2. Так как тип переменной указан, как Animal, компилятор не знает о перегрузке функции Feed(int).
        //    Метод Feed() будет вызван у класса Cat, так как здесь используется перегрузка, а не сокрытие.
        
        DeserializeExample.Deserialize();
        // 3.   Какие незримые механизмы логики могут проявиться тут?
        // 3.1. Реализация сериализации/десериализации в библиотеке, точнее даже в конкретной её версии.
        //      Клиент ожидает определенного поведения от библиотеки,
        //      но как она фактически отработает нельзя точно утверждать, не изучив в достаточной мере её реализацию.
        // 3.2. Передача данных в формате сериализованной строки.
        //      В таком виде формат передачи данных зафиксирован неявно.
        //      То есть в приложении тип данных просто определяется, как string. При этом, если строка передается
        //      между сервисами и приложениями, то везде будут использоваться свои библиотеки
        //      для сериализации/десериализации, которые работают по разным правилам. Это тоже вносит свой вклад
        //      в рост неопределенности.
    }
}