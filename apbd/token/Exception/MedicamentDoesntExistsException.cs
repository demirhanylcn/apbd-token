namespace solution.Exception;

public class MedicamentDoesntExistsException : System.Exception
{
    public MedicamentDoesntExistsException(int medicamentId) : base(
        $"medicament with given id {medicamentId} doesnt exists.")
    {
        
    }
}