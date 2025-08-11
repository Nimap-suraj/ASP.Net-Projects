using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Text;
internal class Program
{           
    private static void Main(string[] args)
    {
        //PatternWonderBizz();
        //Console.WriteLine($" Anagram : {Anagram()}");
        //CountVowels();
        //reverseWord();
        //RemoveDuplicate();
        //int[] arr = { 1, 2, 2, 1 };
        //var data = RemoveDuplicates(arr);
        //Console.WriteLine(data.Length);
        //int[] arr = { 1, 2, 3,4,1,2,3 };
        //Frequency(arr);
        //PrefixMaxString();
        //string input = "abc";
        //string output = "";
        //PrintSubSet(output, input);
    }

    private static void PrefixMaxString()
    {
        string[] strs = { "flower", "flow", "fl" };

        int count1 = 0;
        int trim = 0;

        string pre = strs[0];
        
        int count2 = pre.Length;
        for (int s = 1; s < strs.Length; s++)
        {
            string woard = strs[s];

            for (int i = 0; i < woard.Length; i++)
            {
               
                if (pre[i] == woard[i])
                {
                    count1++;
                }
                else
                {
                    break;
                }
            }
            trim = Math.Min(count1, count2);
            Console.WriteLine(trim);
            count1 = 0;
        }
        Console.WriteLine(pre.Substring(0, trim));

    }
    public static void Frequency(int[] arr)
    {
        var dictionary = new Dictionary<int, int>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (dictionary.ContainsKey(arr[i]))
            {
                dictionary[arr[i]]++;
            }
            else
            {
                dictionary[arr[i]] = 1;
            }
        }
        foreach (var i in dictionary)
        {
            Console.WriteLine($"Key : {i.Key} Value: {i.Value}");
        }
    }
    public static int[] RemoveDuplicates(int[] arr)
    {
        return arr.Distinct().ToArray();
    }
    public static void RemoveDuplicate()
    {
        string str = "shahah";
        var data = new HashSet<char>();
        var res = "";
        foreach (var c in str)
        {
            if(!data.Contains(c))
            {
                data.Add(c);
                res += c;
            }
        }
        Console.WriteLine($"string after: {res}");
    }
    public static void reverseWord()
    {
        string str = "suraj sandeep shah";
        var words = str.Split(" ");
        Array.Reverse(words);
        var ans =  string.Join(" ", words);
        Console.WriteLine("Reverse Words : "+ans);
    }
    public static void CountVowels()
    {
        string str = "IndiaIsmyCOuntry";
        int vowel = 0;
        int consonoat = 0;
        str.ToLower();
        foreach(char ch in str)
        {
            if("aeiou".Contains(ch)) vowel++;
            else if(char.IsLetter(ch)) consonoat++;
        }
        Console.WriteLine($"Vowels: {vowel}, Consonants: {consonoat}");

    }
    public static void PatternWonderBizz()
    {
        int first = 2;
        int second = 3;
        int repeatCount = 1;
        while (true)
        {
            Console.Write(first + " ");
            for (int i = 0; i < repeatCount; i++)
            {
                Console.Write(second + " ");
            }
            repeatCount++;
            Thread.Sleep(1000);
        }
    }
    public static bool Anagram()
    {
        string s1 = "hello";
        string s2 = "hello";
        if(s1.Length != s2.Length)
        {
            return false;
        }
        int[] count = new int[26];
        for (int i = 0; i < s1.Length; i++)
        {
            count[s1[i] - 'a']++;
            count[s2[i] - 'a']--;
        }
        foreach(var val in count)
        {
            if(val != 0)
            {
                return false;
            }
        }
        return true;
    }
    public static void maxAndSecond()
    {
        int[] arr = {9,8,1,2,3,4,5};
        int max = 0;
        int secondmax = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if(arr[i] > max)
            {
                secondmax = max;
                max = arr[i];
            }else if (arr[i] > secondmax && arr[i] != max)
            {
                secondmax = arr[i];
            }
        }
        Console.WriteLine($"maximum Element is {max} and second max :{secondmax}");
    }
    public static void countOfVowel()
    {
        string str = "india is my country";
        //var res = new Dictionary<char, int>();
        int a = 0;
        int e = 0;
        int i = 0;
        int o = 0;
        int u = 0;
        int count = 0;
        foreach (char c in str)
        {
            if(c == 'a')
            {
                a++;
                count++;
            }else if(c == 'e' )
            {
                e++;
                count++;
            }else if(c == 'i')
            {
                i++;
                count++;
            }else if(c == 'o')
            {
                o++;
                count++;
            }else if(c == 'u')
            {
                u++;
                count++;
            }
        }
        Console.WriteLine($"count of Vowels are :{count}");
        Console.WriteLine($"{a} {e}");

    }
    public static void SubString()
    {
        string str = "aaabbcccddddeee";
        int max = 1; 
        int current = 1;
        int start = 0;
        for (int i = 1; i < str.Length; i++)
        {
             
            if (str[i] == str[i - 1])
            {
                current++;
            }else
            {
                current = 1;
            }
            if(current >  max)
            {
                max = current;
                start = i - max + 1; // 2 - 3 + 1
            }
        }
        Console.WriteLine(str.Substring(start,max));
    }
    public static void DecimalToBinary()
    {
        int n = 13;
        int temp = n;
        string ans = "";
        while(temp >= 1)
        {
           int rem = temp % 2;
            ans = ans + rem;
            temp = temp / 2;
        }
        reverseString(ans);
        //Console.WriteLine(ans);
    }
    public static void infinite()
    {
        int n1 = 2;
        int n2 = 3;

        while (true)
        {
            Console.Write($"{n1} {n2} ");
        }
    }
    public static void reverseString(string str)
    {
        StringBuilder sb = new StringBuilder(str);
        int left = 0;
        int right = str.Length - 1;
        while (left < right)
        {
            char temp = sb[left];
            sb[left] = sb[right];
            sb[right] = temp;
            left++;
            right--;
        }
        Console.WriteLine(sb.ToString());
    }
    public static void Palindrome()
    {
        int n = 1213;
        int temp = n;
        int rev = 0;
        while(n > 0)
        {
            int rem = n % 10;
            rev = rev * 10 + rem;
            n = n / 10; 
        }
        if(temp == rev)
        {
            Console.WriteLine("Number is  Palindrome");
        }
        else
        {
            Console.WriteLine("Number is Not Palindrome");
        }
         Console.WriteLine($"The Reverse of {temp} is {rev}");
    }
    public static void Factorial()
    {
        Console.WriteLine("Factorial Program....");
        int n = 5;
        int factorial = 1;
        for(int  i = 1; i <= n; i++)
        {
            factorial *= i;
        }
        Console.WriteLine($"The Factorial of {n} is {factorial}");
    }
    public static void Pattern0()
    {
        int n = 0;
        int i = 1;
        while(n < i && i <= 5)
        {
            Console.Write("*" + " ");
            n++;
            if(n >= i)
            {
                n = 0;
                i++;
                Console.WriteLine();
            }
        }
    }
    public void Pattern1()
    {

        int n = 0;
        int i = 1;
        while (n < i && i < 9)
        {
            if (i >= 5)
            {
                Console.Write(i);
                n++;
            }
            else
            {
                Console.Write("*");
                n++;
            }
            if (n >= i)
            {
                n = 0;
                i++;
                Console.WriteLine();
            }

        }
    }   
    public static void Pattern2()
    {
        int n = 0;
        int i = 1;
        while (n < i && i <= 5)
        {
            Console.Write("*" + " ");
            n++;
            if (n >= 5)
            {
                n = 0;
                i++;
                Console.WriteLine();
            }
        }
    }
    public static void PrintSubSet(string output, string input)
    {
        if (input.Length == 0)
        {
            Console.WriteLine(output);
            return;
        }
        var current = input[0];
        var remaining = input.Substring(1);
        // exclude the first
        PrintSubSet(output, remaining);
        PrintSubSet(output + current, remaining);

    }

}