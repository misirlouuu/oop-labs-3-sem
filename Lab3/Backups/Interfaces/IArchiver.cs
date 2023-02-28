using Backups.Models.Storages;

namespace Backups.Interfaces;

public interface IArchiver
{
    ZipStorage Archive(IReadOnlyCollection<IRepositoryObject> repositoryObjects, IRepository storageRepository);
}