namespace token.Exception;

public class PatientDoesntExistsException : System.Exception
{
    public PatientDoesntExistsException(int patientId) : base($"patient with given id {patientId} doesnt exists.")
    {
    }
}