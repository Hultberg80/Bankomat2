using banko;
using Xunit.Abstractions;

namespace BankomatTest2;

public class BankomatTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public BankomatTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestInsertCard()
    {
        // Setup
        Bankomat bankomat = new Bankomat();
        Account account = new Account();
        Card card = new(account);
        // test
        bool result = bankomat.insertCard(card);
        Assert.True(result);
    }

    [Fact]
    public void TestEnterPin()
    {
        // Setup
        Bankomat bankomat = new Bankomat();
        Account account = new Account();
        Card card = new(account);
        bankomat.insertCard(card);
        // test correct pin
        bool result = bankomat.enterPin("0123");
        Assert.True(result);
        // test correct pin
        result = bankomat.enterPin("1234");
        Assert.False(result);
    }
    
    [Fact]
    public void TestGetMachineBalance()
    {
        Bankomat bankomat = new Bankomat();
        // test
        int balance = bankomat.GetMachineBalance();
        Assert.Equal(11000, balance);
    }
    
    [Theory]
    [InlineData(11000, 5000, 16000)]
    [InlineData(11000, 1000, 12000)]
    [InlineData(11000, 0, 11000)]
    [InlineData(11000, -1000, 11000)]
    public void TheoryTestGetMachineBalance(int initialBalance, int amount, int expectedBalance)
    {
        // Setup
        Bankomat bankomat = new Bankomat();
        // test
       bankomat.AddToMachineBalance(amount);
       int actualBalance = bankomat.GetMachineBalance();
         Assert.Equal(expectedBalance, actualBalance);
    }
    
    [Fact]
    public void TestWithdraw()
    {
        // Setup
        Bankomat bankomat = new Bankomat();
        Account account = new Account();
        Card card = new(account);
        card.ActivateCard(); // isActive = true, behöver inte aktivera kortet
        bankomat.insertCard(card);
        bankomat.enterPin("0123");
        
        // Kontrollera initila saldon
        int initialMachineBalance = bankomat.GetMachineBalance();
        int initialAccountBalance = account.getBalance();
        Assert.Equal(11000, initialMachineBalance);
        Assert.Equal(5000, initialAccountBalance);
        
        // Test uttag
        int withdrawResult = bankomat.withdraw(5000);
        Assert.Equal(5000, withdrawResult);
        
        // Kontrollera saldon efter uttag
        int finalMachineBalance = bankomat.GetMachineBalance();
        int finalAccountBalance = account.getBalance();
        Assert.Equal(initialMachineBalance - 5000, finalMachineBalance);
        Assert.Equal(initialAccountBalance - 5000, finalAccountBalance);
        
        // Ta ut kortet
        bankomat.ejectCard();
        
        // Sätt i kortet igen
        bankomat.insertCard(card);
        bankomat.enterPin("0123");
        
        // Test uttag
        withdrawResult = bankomat.withdraw(7000);
        Assert.Equal(0, withdrawResult);
        
        // Test uttag
        withdrawResult = bankomat.withdraw(6000);
        Assert.Equal(0, withdrawResult);
    }

    [Fact]
    public void TestEjectCard()
    {
        // Setup
        Bankomat bankomat = new Bankomat();
        Account account = new Account();
        Card card = new(account);
        bankomat.insertCard(card);
        bankomat.getMessage();
        // test
        bool result = bankomat.ejectCard();
        
        // Assert
        string message = bankomat.getMessage();
        Assert.Equal("Card removed, don't forget it!", message);
        Assert.False(result);
        
    }

    [Fact]
    public void showBalance()
    {
        Bankomat bankomat = new Bankomat();
        Account account = new();
        Card card = new(account);
        bankomat.insertCard(card);
        bankomat.enterPin("0123");

        account.showBalance();
        Assert.Equal(5000, account.showBalance());
    }
    
    [Fact]
    public void TestValidPin()
    {
        Bankomat bankomat = new Bankomat();
        Account account = new();
        Card card = new(account);
        bankomat.insertCard(card); // Här kan vi ta ut pengar
        bankomat.enterPin("1234"); // Nu måste vi ange pinkod, fast pinkod behöver inte vara "rätt" 0123
        
        // Test uttag inget kort
        int result = bankomat.withdraw(5000);
        Assert.Equal(5000, result);
        
      
    }
    
}