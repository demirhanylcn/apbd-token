namespace solution.Exception;

public class MedicamentGreaterThan10Exception : System.Exception
{
    public MedicamentGreaterThan10Exception() : base("medicament size can not be bigger than 10.")
    {
        
    }
}