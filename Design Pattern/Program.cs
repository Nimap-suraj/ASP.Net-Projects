using System;

public sealed class SingleTon
{
    private static SingleTon _instance;
    private static readonly object obj = new object(); // FIXED: initialized lock object
    private static int count = 0; // Moved outside to persist between calls

    private SingleTon()
    {
        // private constructor
    }

    public static SingleTon GetInstance()
    {
        lock (obj)
        {
            if (_instance == null)
            {
                _instance = new SingleTon();
                count++;
            }
        }
        Console.WriteLine(count);
        return _instance;
    }
}

public class Program
{
    private static void Main(string[] args)
    {
        SingleTon s1 = SingleTon.GetInstance();
        SingleTon s2 = SingleTon.GetInstance();
        SingleTon s3 = SingleTon.GetInstance();
    }
}
