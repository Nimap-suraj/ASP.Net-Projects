public class Employee
{
    public int EmpId;
    public string Name = string.Empty;
    public decimal GrossPay;
    public decimal TaxDeduction = 0.1M;
    public decimal NetSalary;
    public Employee(int EmpId,string Name,decimal GrossPay)
    {
        this.EmpId = EmpId;
        this.Name = Name;
        this.GrossPay = GrossPay;
    }
    void CalculateSalary()
    {
        if(GrossPay >= 30000)
        {
            NetSalary = GrossPay - (TaxDeduction * GrossPay);
            Console.WriteLine("Your Salary is :"+NetSalary);
        }else
        {
            NetSalary = GrossPay;
            Console.WriteLine("Your Salary is :"+NetSalary);
        }
    }
    public void ShowEmployeeDetails() 
    {
        Console.WriteLine("Empployee id is :"+ this.EmpId);
        Console.WriteLine("Empployee Name is :"+ this.Name);
        this.CalculateSalary();
    }
}
public class Program
{
    private static void Main(string[] args)
    {
        Employee suraj = new Employee(1,"suraj Shah",40000);
        suraj.ShowEmployeeDetails();
        Console.WriteLine();
    }
}
