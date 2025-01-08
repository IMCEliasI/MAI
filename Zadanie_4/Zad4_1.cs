using System;

// Интерфейс для численного интегрирования
public interface INumericalIntegration
{
    string MethodName { get; } // Название метода

    (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance); // Метод для вычисления интеграла
}

// Реализация метода левых прямоугольников
public class LeftRectangleMethod : INumericalIntegration
{
    public string MethodName => "Метод левых прямоугольников";

    public (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance)
    {
        int iterations = 2;
        double previousResult;
        double result = 0;

        do
        {
            previousResult = result;
            result = 0;
            double step = (upperBound - lowerBound) / iterations;

            for (int i = 0; i < iterations; i++)
            {
                double x = lowerBound + i * step;
                result += function(x);
            }

            result *= step;
            iterations *= 2;
        } while (Math.Abs(result - previousResult) > tolerance);

        return (result, iterations);
    }
}

// Реализация метода правых прямоугольников
public class RightRectangleMethod : INumericalIntegration
{
    public string MethodName => "Метод правых прямоугольников";

    public (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance)
    {
        int iterations = 2;
        double previousResult;
        double result = 0;

        do
        {
            previousResult = result;
            result = 0;
            double step = (upperBound - lowerBound) / iterations;

            for (int i = 1; i <= iterations; i++)
            {
                double x = lowerBound + i * step;
                result += function(x);
            }

            result *= step;
            iterations *= 2;
        } while (Math.Abs(result - previousResult) > tolerance);

        return (result, iterations);
    }
}

// Реализация метода средних прямоугольников
public class MiddleRectangleMethod : INumericalIntegration
{
    public string MethodName => "Метод средних прямоугольников";

    public (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance)
    { 
        int iterations = 2;
        double previousResult;
        double result = 0;

        do
        {
            previousResult = result;
            result = 0;
            double step = (upperBound - lowerBound) / iterations;

            for (int i = 0; i < iterations; i++)
            {
                double x = lowerBound + (i + 0.5) * step;
                result += function(x);
            }

            result *= step;
            iterations *= 2;
        } while (Math.Abs(result - previousResult) > tolerance);

        return (result, iterations);
    }
}

// Реализация метода трапеций
public class TrapezoidalMethod : INumericalIntegration
{
    public string MethodName => "Метод трапеций";

    public (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance)
    {
        int iterations = 1;
        double previousResult;
        double result = 0;

        do
        {
            previousResult = result;
            result = 0;
            double step = (upperBound - lowerBound) / iterations;

            for (int i = 0; i <= iterations; i++)
            {
                double x = lowerBound + i * step;
                double weight = (i == 0 || i == iterations) ? 0.5 : 1.0;
                result += weight * function(x);
            }

            result *= step;
            iterations *= 2;
        } while (Math.Abs(result - previousResult) > tolerance);

        return (result, iterations);
    }
}

// Реализация метода парабол (Симпсона)
public class SimpsonMethod : INumericalIntegration
{
    public string MethodName => "Метод Симпсона";

    public (double result, int iterations) Integrate(Func<double, double> function, double lowerBound, double upperBound, double tolerance)
    {
        int iterations = 2;
        double previousResult;
        double result = 0;

        do
        {
            previousResult = result;
            result = 0;
            double step = (upperBound - lowerBound) / iterations;

            for (int i = 0; i <= iterations; i++)
            {
                double x = lowerBound + i * step;
                double weight = (i == 0 || i == iterations) ? 1 : (i % 2 == 0 ? 2 : 4);
                result += weight * function(x);
            }

            result *= step / 3.0;
            iterations *= 2;
        } while (Math.Abs(result - previousResult) > tolerance);

        return (result, iterations);
    }
}

// Демонстрация работы методов
public class Zad4_1
{
    public static void Main()
    {
        Func<double, double> function = x => Math.Sin(x); // Подынтегральная функция
        double lowerBound = 0;
        double upperBound = Math.PI/2;
        double tolerance = 1e-6;

        var methods = new INumericalIntegration[]
        {
            new LeftRectangleMethod(),
            new RightRectangleMethod(),
            new MiddleRectangleMethod(),
            new TrapezoidalMethod(),
            new SimpsonMethod()
        };

        var a = DateTime.Now; 

        foreach (var method in methods)
        {
            var startTime = DateTime.Now;
            (double result, int iterations) = method.Integrate(function, lowerBound, upperBound, tolerance);
            var elapsedTime = DateTime.Now - startTime;

            Console.WriteLine($"{method.MethodName}:");
            Console.WriteLine($"Результат: {result:F6}");
            Console.WriteLine($"Количество итераций: {iterations}");
            Console.WriteLine($"Время: {elapsedTime.TotalMilliseconds} мс\n");
        }
    }
}
