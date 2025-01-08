using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class LinkedListTests
{
    [Test]
    public void AddFirst_AddsElementToBeginning()
    {
        var list = new LinkedList<int>();
        list.AddFirst(1);
        list.AddFirst(2);
        Assert.AreEqual(2, list[0]);
        Assert.AreEqual(1, list[1]);
    }

    [Test]
    public void AddLast_AddsElementToEnd()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(2, list[1]);
    }

    [Test]
    public void InsertAt_InsertsElementAtIndex()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(3);
        list.InsertAt(2, 1);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(2, list[1]);
        Assert.AreEqual(3, list[2]);
    }

    [Test]
    public void RemoveFirst_RemovesFirstElement()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.RemoveFirst();
        Assert.AreEqual(2, list[0]);
    }

    [Test]
    public void RemoveLast_RemovesLastElement()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.RemoveLast();
        Assert.AreEqual(1, list[0]);
    }

    [Test]
    public void RemoveAt_RemovesElementAtIndex()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        list.RemoveAt(1);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(3, list[1]);
    }

    [Test]
    public void Reverse_ReversesList()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        var reversed = list.Reverse();
        Assert.AreEqual(3, reversed[0]);
        Assert.AreEqual(2, reversed[1]);
        Assert.AreEqual(1, reversed[2]);
    }

    [Test]
    public void Sort_SortsList()
    {
        var list = new LinkedList<int>();
        list.AddLast(3);
        list.AddLast(1);
        list.AddLast(2);
        list.Sort(SortOrder.Ascending, SortAlgorithm.Quick);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(2, list[1]);
        Assert.AreEqual(3, list[2]);
    }

    [Test]
    public void ForEach_AppliesActionToEachElement()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        int sum = 0;
        list.ForEach(x => sum += x);
        Assert.AreEqual(6, sum);
    }

    [Test]
    public void Map_MapsElementsToNewType()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        var mapped = list.Map(x => x.ToString());
        Assert.AreEqual("1", mapped[0]);
        Assert.AreEqual("2", mapped[1]);
        Assert.AreEqual("3", mapped[2]);
    }

    [Test]
    public void Equals_ReturnsTrueForEqualLists()
    {
        var list1 = new LinkedList<int>();
        list1.AddLast(1);
        list1.AddLast(2);
        var list2 = new LinkedList<int>();
        list2.AddLast(1);
        list2.AddLast(2);
        Assert.IsTrue(list1.Equals(list2));
    }

    [Test]
    public void Clone_CreatesDeepCopy()
    {
        var list = new LinkedList<int>();
    list.AddLast(1);
    list.AddLast(2);
    list.AddLast(3);

    var clone = (LinkedList<int>)list.Clone();

    Assert.AreEqual(3, clone.Count());
    Assert.AreEqual(1, clone[0]);
    Assert.AreEqual(2, clone[1]);
    Assert.AreEqual(3, clone[2]);

    // Ensure the original list is not affected by changes to the clone
    clone.AddFirst(0);
    Assert.AreEqual(3, list.Count());
    }

    [Test]
    public void GetEnumerator_IteratesOverElements()
    {
        var list = new LinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);
        int[] expected = { 1, 2, 3 };
        int index = 0;
        foreach (var item in list)
        {
            Assert.AreEqual(expected[index++], item);
        }
    }

    [Test]
    public void OperatorEquals_ReturnsTrueForEqualLists()
    {
        var list1 = new LinkedList<int>();
        list1.AddLast(1);
        list1.AddLast(2);
        var list2 = new LinkedList<int>();
        list2.AddLast(1);
        list2.AddLast(2);
        Assert.IsTrue(list1 == list2);
    }

    [Test]
    public void OperatorNotEquals_ReturnsTrueForDifferentLists()
    {
        var list1 = new LinkedList<int>();
        list1.AddLast(1);
        list1.AddLast(2);
        var list2 = new LinkedList<int>();
        list2.AddLast(1);
        list2.AddLast(3);
        Assert.IsTrue(list1 != list2);
    }

    [Test]
    public void OperatorMultiply_ReturnsListWithMultipliedElements()
    {
        var list1 = new LinkedList<int>();
        list1.AddLast(1);
        list1.AddLast(2);
        var list2 = new LinkedList<int>();
        list2.AddLast(3);
        list2.AddLast(4);
        var result = list1 * list2;
        Assert.AreEqual(3, result[0]);
        Assert.AreEqual(8, result[1]);
    }
}
