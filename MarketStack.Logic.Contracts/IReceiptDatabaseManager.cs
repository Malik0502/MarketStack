namespace MarketStack.Logic.Contracts;

public interface IReceiptDatabaseManager
{
    public Task Insert(string ticketId, string languageCode = "de-DE");

    public Task InsertReceipts();
}