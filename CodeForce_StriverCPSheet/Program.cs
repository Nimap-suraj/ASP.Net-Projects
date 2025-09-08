public class Program
{
    private static void Main(string[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());
        int x = 0;
        while (n > 0)
        {
            var input = Console.ReadLine();
            if (input[1] == '+')
            {
                x++;
            }
            else
            {
                 x--;
            }
            n--;
        }
        Console.WriteLine(x);
    }
}