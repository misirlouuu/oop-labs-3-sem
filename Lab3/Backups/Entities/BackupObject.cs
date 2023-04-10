using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Entities;

public class BackupObject
{
    private readonly IRepository _repository;
    private readonly string _path;

    public BackupObject(IRepository repository, string path)
    {
        ArgumentNullException.ThrowIfNull(repository);

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentNullException(path);
        }

        _repository = repository;
        _path = path;
    }

    public IRepositoryObject GetRepositoryObject()
    {
        return _repository.GetRepositoryObject(_path);
    }
}