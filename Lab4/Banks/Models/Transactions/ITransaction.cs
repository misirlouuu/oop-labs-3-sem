namespace Banks.Models.Transactions;

public interface ITransaction
{
    DateTime CreationDate { get; }
    Guid Id { get; }
    bool IsCanceled { get; }

    void Execute();
    void Undo();

    bool Equals(object obj);
}