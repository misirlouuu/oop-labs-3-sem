using Backups.Composites;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repositories;

namespace Backups.Entities;

public class BackupObject
{
    private readonly IRepository _repository;
    private readonly string _relativePath;

    public BackupObject(IRepository repository, string relativePath)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;

        if (string.IsNullOrWhiteSpace(relativePath))
            throw new ArgumentNullException(relativePath);
        _relativePath = relativePath;

        if (!_repository.Exists(relativePath))
            throw new BackupObjectIsNotFoundException();
    }

    public IRepositoryObject GetRepositoryObject() => _repository.GetRepositoryObject(_relativePath);
}