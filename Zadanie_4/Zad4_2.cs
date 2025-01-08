using System;
using System.Collections.Generic;

public class LinkedListNode<T>
{
    public T Value { get; set; }
    public LinkedListNode<T> Next { get; set; }

    public LinkedListNode(T value)
    {
        Value = value;
        Next = null;
    }
}

public class LinkedList<T> : ICloneable, IEnumerable<T>, IEquatable<LinkedList<T>>
{
    private LinkedListNode<T> head;

    public LinkedList()
    {
        head = null;
    }

    public LinkedList(LinkedList<T> list)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        if (list.head == null) return;

        head = new LinkedListNode<T>(list.head.Value);
        LinkedListNode<T> current = head;
        LinkedListNode<T> listCurrent = list.head.Next;

        while (listCurrent != null)
        {
            current.Next = new LinkedListNode<T>(listCurrent.Value);
            current = current.Next;
            listCurrent = listCurrent.Next;
        }
    }

    public LinkedList(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        IEnumerator<T> enumerator = collection.GetEnumerator();
        if (enumerator.MoveNext())
        {
            head = new LinkedListNode<T>(enumerator.Current);
            LinkedListNode<T> current = head;

            while (enumerator.MoveNext())
            {
                current.Next = new LinkedListNode<T>(enumerator.Current);
                current = current.Next;
            }
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            LinkedListNode<T> current = head;
            for (int i = 0; i < index; i++)
            {
                if (current == null) throw new ArgumentOutOfRangeException(nameof(index));
                current = current.Next;
            }

            if (current == null) throw new ArgumentOutOfRangeException(nameof(index));
            return current.Value;
        }
    }

    public void AddFirst(T value)
    {
        LinkedListNode<T> newNode = new LinkedListNode<T>(value);
        newNode.Next = head;
        head = newNode;
    }

    public void AddLast(T value)
    {
        LinkedListNode<T> newNode = new LinkedListNode<T>(value);
        if (head == null)
        {
            head = newNode;
        }
        else
        {
            LinkedListNode<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = newNode;
        }
    }

    public void InsertAt(T value, int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
        {
            AddFirst(value);
            return;
        }

        LinkedListNode<T> current = head;
        for (int i = 0; i < index - 1; i++)
        {
            if (current == null) throw new ArgumentOutOfRangeException(nameof(index));
            current = current.Next;
        }

        if (current == null) throw new ArgumentOutOfRangeException(nameof(index));

        LinkedListNode<T> newNode = new LinkedListNode<T>(value);
        newNode.Next = current.Next;
        current.Next = newNode;
    }

    public void RemoveFirst()
    {
        if (head == null) throw new InvalidOperationException("The list is empty.");
        head = head.Next;
    }

    public void RemoveLast()
    {
        if (head == null) throw new InvalidOperationException("The list is empty.");

        if (head.Next == null)
        {
            head = null;
            return;
        }

        LinkedListNode<T> current = head;
        while (current.Next.Next != null)
        {
            current = current.Next;
        }
        current.Next = null;
    }

    public void RemoveAt(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

        if (index == 0)
        {
            RemoveFirst();
            return;
        }

        LinkedListNode<T> current = head;
        for (int i = 0; i < index - 1; i++)
        {
            if (current == null || current.Next == null) throw new ArgumentOutOfRangeException(nameof(index));
            current = current.Next;
        }

        if (current.Next == null) throw new ArgumentOutOfRangeException(nameof(index));
        current.Next = current.Next.Next;
    }

    public LinkedList<T> Reverse()
    {
        LinkedList<T> reversedList = new LinkedList<T>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            reversedList.AddFirst(current.Value);
            current = current.Next;
        }
        return reversedList;
    }

    public static LinkedList<T> operator !(LinkedList<T> list)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        return list.Reverse();
    }

    public static LinkedList<T> Concat(LinkedList<T> first, LinkedList<T> second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        LinkedList<T> result = new LinkedList<T>(first);
        LinkedListNode<T> current = result.head;

        if (current == null)
        {
            result.head = new LinkedListNode<T>(second.head.Value);
            current = result.head;
        }
        else
        {
            while (current.Next != null)
            {
                current = current.Next;
            }
        }

        LinkedListNode<T> secondCurrent = second.head;
        while (secondCurrent != null)
        {
            current.Next = new LinkedListNode<T>(secondCurrent.Value);
            current = current.Next;
            secondCurrent = secondCurrent.Next;
        }

        return result;
    }

    public static LinkedList<T> operator +(LinkedList<T> first, LinkedList<T> second)
    {
        return Concat(first, second);
    }

    public static LinkedList<T> Intersect(LinkedList<T> first, LinkedList<T> second, IEqualityComparer<T> comparer)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        if (comparer == null) throw new ArgumentNullException(nameof(comparer));

        LinkedList<T> result = new LinkedList<T>();
        HashSet<T> set = new HashSet<T>(comparer);
        LinkedListNode<T> secondCurrent = second.head;
        while (secondCurrent != null)
        {
            set.Add(secondCurrent.Value);
            secondCurrent = secondCurrent.Next;
        }

        LinkedListNode<T> current = first.head;
        while (current != null)
        {
            if (set.Contains(current.Value))
            {
                result.AddLast(current.Value);
            }
            current = current.Next;
        }

        return result;
    }

    public static LinkedList<T> operator &(LinkedList<T> first, LinkedList<T> second)
    {
        return Intersect(first, second, EqualityComparer<T>.Default);
    }

    public static LinkedList<T> Union(LinkedList<T> first, LinkedList<T> second, IEqualityComparer<T> comparer)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        if (comparer == null) throw new ArgumentNullException(nameof(comparer));

        LinkedList<T> result = new LinkedList<T>();
        HashSet<T> set = new HashSet<T>(comparer);

        LinkedListNode<T> current = first.head;
        while (current != null)
        {
            if (set.Add(current.Value))
            {
                result.AddLast(current.Value);
            }
            current = current.Next;
        }

        current = second.head;
        while (current != null)
        {
            if (set.Add(current.Value))
            {
                result.AddLast(current.Value);
            }
            current = current.Next;
        }

        return result;
    }

    public static LinkedList<T> operator |(LinkedList<T> first, LinkedList<T> second)
    {
        return Union(first, second, EqualityComparer<T>.Default);
    }

    public static LinkedList<T> Subtract(LinkedList<T> first, LinkedList<T> second, IEqualityComparer<T> comparer)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));
        if (comparer == null) throw new ArgumentNullException(nameof(comparer));

        LinkedList<T> result = new LinkedList<T>();
        HashSet<T> set = new HashSet<T>(comparer);
        LinkedListNode<T> secondCurrent = second.head;
        while (secondCurrent != null)
        {
            set.Add(secondCurrent.Value);
            secondCurrent = secondCurrent.Next;
        }

        LinkedListNode<T> current = first.head;
        while (current != null)
        {
            if (!set.Contains(current.Value))
            {
                result.AddLast(current.Value);
            }
            current = current.Next;
        }

        return result;
    }

    public static LinkedList<T> operator -(LinkedList<T> first, LinkedList<T> second)
    {
        return Subtract(first, second, EqualityComparer<T>.Default);
    }

    public void Sort(SortOrder order, SortAlgorithm algorithm)
    {
        if (head == null) return;

        List<T> list = new List<T>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        var array = list.ToArray();
        Array.Sort(array, (x, y) => 
        {
            int result = Comparer<T>.Default.Compare(x, y);
            return order == SortOrder.Ascending ? result : -result;
        });
        list = new List<T>(array);

        current = head;
        foreach (var item in list)
        {
            current.Value = item;
            current = current.Next;
        }
    }

    public void Sort(SortOrder order, SortAlgorithm algorithm, IComparer<T> comparer)
    {
        if (head == null) return;

        List<T> list = new List<T>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        list.ToArray().Sort(order, algorithm, comparer);

        current = head;
        foreach (var item in list)
        {
            current.Value = item;
            current = current.Next;
        }
    }

    public void Sort(SortOrder order, SortAlgorithm algorithm, Comparer<T> comparer)
    {
        if (head == null) return;

        List<T> list = new List<T>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        list.ToArray().Sort(order, algorithm, comparer);

        current = head;
        foreach (var item in list)
        {
            current.Value = item;
            current = current.Next;
        }
    }

    public void Sort(SortOrder order, SortAlgorithm algorithm, Comparison<T> comparison)
    {
        if (head == null) return;

        List<T> list = new List<T>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            list.Add(current.Value);
            current = current.Next;
        }

        list.ToArray().Sort(order, algorithm, comparison);

        current = head;
        foreach (var item in list)
        {
            current.Value = item;
            current = current.Next;
        }
    }

    public void ForEach(Action<T> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        LinkedListNode<T> current = head;
        while (current != null)
        {
            action(current.Value);
            current = current.Next;
        }
    }

    public LinkedList<TResult> Map<TResult>(Func<T, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        LinkedList<TResult> result = new LinkedList<TResult>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            result.AddLast(func(current.Value));
            current = current.Next;
        }
        return result;
    }

    public static bool operator ==(LinkedList<T> first, LinkedList<T> second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) return false;

        LinkedListNode<T> currentFirst = first.head;
        LinkedListNode<T> currentSecond = second.head;

        while (currentFirst != null && currentSecond != null)
        {
            if (!EqualityComparer<T>.Default.Equals(currentFirst.Value, currentSecond.Value))
            {
                return false;
            }
            currentFirst = currentFirst.Next;
            currentSecond = currentSecond.Next;
        }

        return currentFirst == null && currentSecond == null;
    }

    public static bool operator !=(LinkedList<T> first, LinkedList<T> second)
    {
        return !(first == second);
    }

    public static LinkedList<T> operator *(LinkedList<T> first, LinkedList<T> second)
    {
        if (first == null) throw new ArgumentNullException(nameof(first));
        if (second == null) throw new ArgumentNullException(nameof(second));

        LinkedList<T> result = new LinkedList<T>();
        LinkedListNode<T> currentFirst = first.head;
        LinkedListNode<T> currentSecond = second.head;

        while (currentFirst != null && currentSecond != null)
        {
            dynamic valueFirst = currentFirst.Value;
            dynamic valueSecond = currentSecond.Value;
            result.AddLast(valueFirst * valueSecond);

            currentFirst = currentFirst.Next;
            currentSecond = currentSecond.Next;
        }

        return result;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        LinkedListNode<T> current = head;
        while (current != null)
        {
            hash = hash * 31 + (current.Value == null ? 0 : current.Value.GetHashCode());
            current = current.Next;
        }
        return hash;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        LinkedList<T> other = (LinkedList<T>)obj;
        return this == other;
    }

    public override string ToString()
    {
        List<string> values = new List<string>();
        LinkedListNode<T> current = head;
        while (current != null)
        {
            values.Add(current.Value?.ToString() ?? "null");
            current = current.Next;
        }
        return string.Join(" -> ", values);
    }

    public bool Equals(LinkedList<T> other)
    {
        return this == other;
    }

    public object Clone()
    {
        return new LinkedList<T>(this);
    }

    public IEnumerator<T> GetEnumerator()
    {
        LinkedListNode<T> current = head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
