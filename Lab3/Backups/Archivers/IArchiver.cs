using Backups.Composites;
using Backups.Repositories;
using Backups.Storages;

namespace Backups.Archivers;

public interface IArchiver
{
    ZipStorage Archive(IEnumerable<IRepositoryObject> repositoryObjects, IRepository storageRepository);
}