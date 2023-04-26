namespace asyncAwaitPoc;

public class Handler 
{
    private readonly IRepository _repository;

    public Handler( IRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> ExecuteDoubleSave()
    {
        await _repository.Save();

        await _repository.Save();
        return "OK";
    }

    public async Task<string> ExecuteSave()
    {
        await _repository.Save();
        return "OK";
    }
}