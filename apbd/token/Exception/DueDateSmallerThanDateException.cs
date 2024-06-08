namespace token.Exception;

public class DueDateSmallerThanDateException : System.Exception
{
    public DueDateSmallerThanDateException(DateTime dueDate, DateTime date) : base(
        $"dueDate{dueDate} is smaller than date {date}")
    {
    }
}