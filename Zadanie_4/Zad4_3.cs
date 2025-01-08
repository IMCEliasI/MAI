
using System;

public interface IPrimalityTest
{
    string TestName {get;}
    // Метод для проверки числа на простоту с заданной вероятностью
    bool IsPrime(int number, double minProbability);
}

public abstract class ProbabilisticPrimalityTest : IPrimalityTest
{
    public abstract string TestName {get;}
    // Абстрактный метод для выполнения конкретного теста простоты
    protected abstract bool RunTest(int number, int randomParam);

    // Метод для запуска цикла с псевдослучайными параметрами
    public bool IsPrime(int number, double minProbability)
    {
        int iterations = (int)Math.Ceiling(Math.Log(1 / (1 - minProbability)));
        int successfulTests = 0;

        Random random = new Random();
        for (int i = 0; i < iterations; i++)
        {
            int randomParam = random.Next(2, number - 1);  // Генерация случайного параметра
            if (RunTest(number, randomParam))
            {
                successfulTests++;
            }
        }

        // Если доля успешных тестов выше минимальной вероятности, число считается простым
        return (successfulTests / (double)iterations) >= minProbability;
    }
}

// Детерминированный тест простоты (для примера, проверка делимости на простые числа)
public class DeterministicPrimalityTest : ProbabilisticPrimalityTest
{
    public override string TestName => "Детерминированный тест простоты";
    protected override bool RunTest(int number, int randomParam)
    {
        if (number < 2)
            return false;

        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }
}

// Тест Ферма
public class FermatPrimalityTest : ProbabilisticPrimalityTest
{
    public override string TestName => "Тест Ферма";
    protected override bool RunTest(int number, int randomParam)
    {
        if (number == randomParam || number == 1)
            return false;

        // Проверка на основание Ферма (a^(n-1) ≡ 1 (mod n))
        return PowerMod(randomParam, number - 1, number) == 1;
    }

    private int PowerMod(int baseValue, int exponent, int modulus)
    {
        int result = 1;
        baseValue = baseValue % modulus;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * baseValue) % modulus;

            exponent = exponent >> 1;
            baseValue = (baseValue * baseValue) % modulus;
        }

        return result;
    }
}

// Тест Соловея-Штрассена
public class SolovayStrassenPrimalityTest : ProbabilisticPrimalityTest
{
    public override string TestName => "Тест Соловея-Штрассена";
    protected override bool RunTest(int number, int randomParam)
    {
        if (number == 1 || number == 2)
            return true;

        if (PowerMod(randomParam, (number - 1) / 2, number) != JacobiSymbol(randomParam, number))
            return false;

        return true;
    }

    // Вычисление символа Якоби
    private int JacobiSymbol(int a, int n)
    {
        int result = 1;

        while (a != 0)
        {
            while (a % 2 == 0)
            {
                a /= 2;
                if (n % 8 == 3 || n % 8 == 5)
                    result = -result;
            }

            var temp = a;
            a = n % a;
            n = temp;

            if (a % 4 == 3 && n % 4 == 3)
                result = -result;
        }

        if (n == 1)
            return result;

        return 0;
    }

    // Возведение в степень по модулю
    private int PowerMod(int baseValue, int exponent, int modulus)
    {
        int result = 1;
        baseValue = baseValue % modulus;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * baseValue) % modulus;

            exponent = exponent >> 1;
            baseValue = (baseValue * baseValue) % modulus;
        }

        return result;
    }
}

    // Тест Миллера-Рабина
public class MillerRabinPrimalityTest : ProbabilisticPrimalityTest
{
    public override string TestName => "Тест Миллера-Рабина";
    protected override bool RunTest(int number, int randomParam)
    {
        if (number == randomParam || number == 1)
            return false;

        int d = number - 1;
        int r = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            r++;
        }

        int x = PowerMod(randomParam, d, number);
        if (x == 1 || x == number - 1)
            return true;

        for (int i = 0; i < r - 1; i++)
        {
            x = PowerMod(x, 2, number);
            if (x == number - 1)
                return true;
        }

        return false;
    }

    // Возведение в степень по модулю
    private int PowerMod(int baseValue, int exponent, int modulus)
    {
        int result = 1;
        baseValue = baseValue % modulus;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * baseValue) % modulus;

            exponent = exponent >> 1;
            baseValue = (baseValue * baseValue) % modulus;
        }

        return result;
    }
}

// НОД двух чисел
public static class PrimalityHelpers
{
    public static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
}

public class Zad4_3
{
    public static void Main(string[] args)
    {
        int number = 4;
        double minProbability = 0.95;

        // Выбор теста
        var tests = new IPrimalityTest[]
        {
            new DeterministicPrimalityTest(),
            new FermatPrimalityTest(),
            new SolovayStrassenPrimalityTest(),
            new MillerRabinPrimalityTest()
        };

        foreach (var test in tests)
        {
            bool isPrime = test.IsPrime(number, minProbability);
            Console.WriteLine($"{test.TestName}: {number} {(isPrime ? "простое" : "непростое")}");
        }
    }
}