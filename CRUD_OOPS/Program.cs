using CRUD_OOPS;
using System.Data;

public class Program
{
    static List<Employee> employees = new List<Employee>();

    public static void Main(string[] args)
    {

        int choice;
       
        do
        {
            Console.WriteLine("CRUD Operation:");
            Console.WriteLine("1: Create");
            Console.WriteLine("2: Read");
            Console.WriteLine("3: Update");
            Console.WriteLine("4: Delete");
            Console.WriteLine("5: Exit");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Create();
                    break;
                case 2:
                    Read();
                    break;
                case 3:
                    Update();
                    break;
                case 4:
                    Delete();
                    break;
                case 5:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;

            }
        } while (choice != 5);
        
    }

    private static void Delete()
    {
        Console.WriteLine("enter id of employee which need to Delete!");
        var id = Convert.ToInt32(Console.ReadLine());
        var employee = employees.Find(x => x.Id == id);
        if (employee != null)
        {
            employees.Remove(employee);
            Console.WriteLine("employee Deleted !");
        }
        else
        {
            Console.WriteLine("Employee Not Found!");
        }
    }

    private static void Update()
    {
        Console.WriteLine("enter id of employee which need to update!");
        var id = Convert.ToInt32(Console.ReadLine());
        var employee  = employees.Find(x => x.Id == id);
        if(employee != null)
        {
            Console.WriteLine("Enter Name");
            employee.Name = Console.ReadLine();

            Console.WriteLine("Enter Salary");
            employee.Salary = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("employee update !");
        }
        else
        {
            Console.WriteLine("Employee Not Found!");
        }
    }

    private static void Read()
    {

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees available.");
            return;
        }
        var list = employees.ToList();
        foreach (Employee e in list)
        {
            Console.WriteLine($"id: {e.Id} Name: {e.Name} Salary: {e.Salary}");
        }
    }

    private static void Create()
    {
        Console.WriteLine("Enter Id");
        var id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter Name");
        var name = Console.ReadLine();
        Console.WriteLine("Enter Salary");
        var salary = Convert.ToDecimal(Console.ReadLine());
        var employee = new Employee
        {
            Id = id,
            Name = name,
            Salary = salary
        };
        employees.Add(employee);
        Console.WriteLine("Employee added");
    }
}