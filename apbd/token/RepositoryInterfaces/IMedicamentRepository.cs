namespace solution.RepositoryInterfaces;

public interface IMedicamentRepository
{
    public Task<bool> CheckMedicamentExists(int medicamentId);

}