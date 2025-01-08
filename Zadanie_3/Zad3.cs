using System;
using System.Collections.Generic;

public enum SortOrder
{
    Ascending,
    Descending
}

public enum SortAlgorithm
{
    Insertion,
    Selection,
    Heap,
    Quick,
    Merge
}

public static class ArrayExtensions
{
    // Сортировка без внешнего правила сравнения
    public static void Sort<T>(this T[] array, SortOrder order, SortAlgorithm algorithm) where T : IComparable<T>
    {
        Comparison<T> comparison = (x, y) => order == SortOrder.Ascending ? x.CompareTo(y) : y.CompareTo(x);
        array.Sort(order, algorithm, comparison);
    }

    // Сортировка с использованием IComparer<T>
    public static void Sort<T>(this T[] array, SortOrder order, SortAlgorithm algorithm, IComparer<T> comparer)
    {
        Comparison<T> comparison = (x, y) => order == SortOrder.Ascending ? comparer.Compare(x, y) : comparer.Compare(y, x);
        array.Sort(order, algorithm, comparison);
    }

    // Сортировка с использованием Comparer<T>
    public static void Sort<T>(this T[] array, SortOrder order, SortAlgorithm algorithm, Comparer<T> comparer)
    {
        array.Sort(order, algorithm, (x, y) => order == SortOrder.Ascending ? comparer.Compare(x, y) : comparer.Compare(y, x));
    }

    // Сортировка с использованием делегата Comparison<T>
    // P.S. Я знаю что order можно тут убрать, так как его функционал уже включен в делегат Comparison<T>, но по тз он должен быть тут.
    public static void Sort<T>(this T[] array, SortOrder order, SortAlgorithm algorithm, Comparison<T> comparison)
    {
        switch (algorithm)
        {
            case SortAlgorithm.Insertion:
                InsertionSort(array, comparison);
                break;
            case SortAlgorithm.Selection:
                SelectionSort(array, comparison);
                break;
            case SortAlgorithm.Heap:
                HeapSort(array, comparison);
                break;
            case SortAlgorithm.Quick:
                QuickSort(array, 0, array.Length - 1, comparison);
                break;
            case SortAlgorithm.Merge:
                array = MergeSort(array, comparison);
                break;
            default:
                throw new ArgumentException("Протри глаза, ты используешь не известный алгоритм сортировки");
        }
    }

    // Реализация сортировки вставками
    private static void InsertionSort<T>(T[] array, Comparison<T> comparison)
    {
        for (int i = 1; i < array.Length; i++)
        {
            var key = array[i];
            int j = i - 1;

            while (j >= 0 && comparison(array[j], key) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }

            array[j + 1] = key;
        }
    }

    // Реализация сортировки выбором
    private static void SelectionSort<T>(T[] array, Comparison<T> comparison)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < array.Length; j++)
            {
                if (comparison(array[j], array[minIndex]) < 0)
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }
        }
    }

    // Реализация пирамидальной сортировки
    private static void HeapSort<T>(T[] array, Comparison<T> comparison)
    {
        void Heapify(T[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && comparison(arr[left], arr[largest]) > 0)
            {
                largest = left;
            }

            if (right < n && comparison(arr[right], arr[largest]) > 0)
            {
                largest = right;
            }

            if (largest != i)
            {
                (arr[i], arr[largest]) = (arr[largest], arr[i]);
                Heapify(arr, n, largest);
            }
        }

        int n = array.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(array, n, i);
        }

        for (int i = n - 1; i > 0; i--)
        {
            (array[0], array[i]) = (array[i], array[0]);
            Heapify(array, i, 0);
        }
    }

    // Реализация быстрой сортировки (Quick Sort)
    private static void QuickSort<T>(T[] array, int low, int high, Comparison<T> comparison)
    {
        if (low < high)
        {
            int pivotIndex = Partition(array, low, high, comparison);

            QuickSort(array, low, pivotIndex - 1, comparison);
            QuickSort(array, pivotIndex + 1, high, comparison);
        }
    }

    private static int Partition<T>(T[] array, int low, int high, Comparison<T> comparison)
    {
        var pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (comparison(array[j], pivot) <= 0)
            {
                i++;
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        (array[i + 1], array[high]) = (array[high], array[i + 1]);
        return i + 1;
    }

    // Реализация сортировки слиянием
    private static T[] MergeSort<T>(T[] array, Comparison<T> comparison)
    {
        if (array.Length <= 1)
        {
            return array;
        }

        int mid = array.Length / 2;
        var left = MergeSort(array[..mid], comparison);
        var right = MergeSort(array[mid..], comparison);

        return Merge(left, right, comparison);
    }

    private static T[] Merge<T>(T[] left, T[] right, Comparison<T> comparison)
    {
        var result = new List<T>();
        int i = 0, j = 0;

        while (i < left.Length && j < right.Length)
        {
            if (comparison(left[i], right[j]) <= 0)
            {
                result.Add(left[i++]);
            }
            else
            {
                result.Add(right[j++]);
            }
        }

        result.AddRange(left[i..]);
        result.AddRange(right[j..]);

        return result.ToArray();
    }
}

// Пример использования
public class Program
{
    public static void Main()
    {
        int[] array = { -5, 2, 9, 1, 5, 6 };


        // Сортировка по возрастанию с использованием быстрой сортировки
        array.Sort(SortOrder.Ascending, SortAlgorithm.Quick);
        Console.WriteLine("Быстрая сортировка (Возрастание): " + string.Join(", ", array));

        // Сортировка по убыванию с использованием сортировки вставками
        array.Sort(SortOrder.Descending, SortAlgorithm.Insertion);
        Console.WriteLine("Сортировка вставками (Убывание): " + string.Join(", ", array));

        // Сортировка по убыванию с использованием сортировки выбором
        array.Sort(SortOrder.Descending, SortAlgorithm.Selection);
        Console.WriteLine("Сортировка выбором (Убывание): " + string.Join(", ", array));

        // Сортировка по убыванию с использованием пирамидальной сортировки 
        array.Sort(SortOrder.Descending, SortAlgorithm.Heap);
        Console.WriteLine("Пирамидальная сортировка (Убывание): " + string.Join(", ", array));

        // Сортировка по убыванию с использованием сортировки слиянием
        array.Sort(SortOrder.Descending, SortAlgorithm.Merge);
        Console.WriteLine("Сортировка слиянием (Убывание): " + string.Join(", ", array));

        // Сортировка с использованием IComparer<T>
        array.Sort(SortOrder.Ascending, SortAlgorithm.Selection, Comparer<int>.Default);
        Console.WriteLine("Сортировка выбором (Возрастание): " + string.Join(", ", array));

        // Сортировка с использованием делегата Comparison<T>
        array.Sort(SortOrder.Descending, SortAlgorithm.Merge, (x, y) => x.CompareTo(y));
        Console.WriteLine("Сортировка слиянием (Убывание): " + string.Join(", ", array));
    }
}