using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    // Генерация всех сочетаний с повторениями
    public static IEnumerable<IEnumerable<T>> GetCombinationsWithRepetition<T>(
        this IEnumerable<T> source, int k, IEqualityComparer<T> comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (k < 0) throw new ArgumentOutOfRangeException(nameof(k));

        var list = source.ToList();
        ValidateNoDuplicates(list, comparer ?? EqualityComparer<T>.Default);

        return GetCombinationsWithRepetitionInternal(list, k);
    }

    private static IEnumerable<IEnumerable<T>> GetCombinationsWithRepetitionInternal<T>(List<T> list, int k)
    {
        if (k == 0) return new List<List<T>> { new List<T>() };

        return list.SelectMany((item, index) =>
            GetCombinationsWithRepetitionInternal(list.Skip(index).ToList(), k - 1)
            .Select(combination => new[] { item }.Concat(combination)));
    }

    // Генерация всех сочетаний без повторений
    public static IEnumerable<IEnumerable<T>> GetCombinationsWithoutRepetition<T>(
        this IEnumerable<T> source, int k, IEqualityComparer<T> comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (k < 0) throw new ArgumentOutOfRangeException(nameof(k));

        var list = source.ToList();
        ValidateNoDuplicates(list, comparer ?? EqualityComparer<T>.Default);

        return GetCombinationsWithoutRepetitionInternal(list, k);
    }

    private static IEnumerable<IEnumerable<T>> GetCombinationsWithoutRepetitionInternal<T>(List<T> list, int k)
    {
        if (k == 0) return new List<List<T>> { new List<T>() };

        return list.SelectMany((item, index) =>
            GetCombinationsWithoutRepetitionInternal(list.Skip(index + 1).ToList(), k - 1)
            .Select(combination => new[] { item }.Concat(combination)));
    }

    // Генерация всех подмножеств
    public static IEnumerable<IEnumerable<T>> GetSubsets<T>(
        this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var list = source.ToList();
        ValidateNoDuplicates(list, comparer ?? EqualityComparer<T>.Default);

        return GetSubsetsInternal(list);
    }

    private static IEnumerable<IEnumerable<T>> GetSubsetsInternal<T>(List<T> list)
    {
        return Enumerable.Range(0, 1 << list.Count)
            .Select(mask => list.Where((_, index) => (mask & (1 << index)) != 0));
    }

    // Генерация всех перестановок
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(
        this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var list = source.ToList();
        ValidateNoDuplicates(list, comparer ?? EqualityComparer<T>.Default);

        return GetPermutationsInternal(list);
    }

    private static IEnumerable<IEnumerable<T>> GetPermutationsInternal<T>(List<T> list)
    {
        if (list.Count == 0) return new List<List<T>> { new List<T>() };

        return list.SelectMany((item, index) =>
            GetPermutationsInternal(list.Where((_, i) => i != index).ToList())
            .Select(permutation => new[] { item }.Concat(permutation)));
    }

    // Проверка на наличие дубликатов
    private static void ValidateNoDuplicates<T>(List<T> list, IEqualityComparer<T> comparer)
    {
        var set = new HashSet<T>(comparer);
        foreach (var item in list)
        {
            if (!set.Add(item))
            {
                throw new ArgumentException("В списке присутствуют дубликаты.");
            }
        }
    }
}

// Пример использования
public class Program
{
    public static void Main()
    {
        var input_data = new[] { 1, 2, 3, 4 };
        var input_k = 2;

        Console.WriteLine("Сочетания с повторениями:");
        foreach (var combination in input_data.GetCombinationsWithRepetition(input_k))
        {
            Console.WriteLine(string.Join(", ", combination));
        }

        Console.WriteLine("\nСочетания без повторений:");
        foreach (var combination in input_data.GetCombinationsWithoutRepetition(input_k))
        {
            Console.WriteLine(string.Join(", ", combination));
        }

        Console.WriteLine("\nВсе подмножества:");
        foreach (var subset in input_data.GetSubsets())
        {
            Console.WriteLine(string.Join(", ", subset));
        }

        Console.WriteLine("\nВсе перестоновки:");
        foreach (var permutation in input_data.GetPermutations())
        {
            Console.WriteLine(string.Join(", ", permutation));
        }
    }
}