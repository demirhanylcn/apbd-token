namespace token.Exception;

public class DoctorDoesntExistsException : System.Exception
{
    public DoctorDoesntExistsException(int doctorId) : base($"given doctor id {doctorId} does not exists")
    {
    }
}