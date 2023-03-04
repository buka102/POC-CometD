namespace POC;

public interface IPOCService
{
    Task<string> CreateAccount(string accountName);

    Task TestCometD();
}
