using LegacyApp;

namespace LegacyAppTests;

public class AddUserTests
{
    [Fact]
    public void AddUser_Should_Return_False_when_Missing_FirstName()
    {
        int clientId = 1;
        string firstName = null;
        string lastName = null;
        string email = "kowalski@wp.pl";
        var dateOfBirth = new DateTime(1980, 1, 1);
        var userService = new UserService();

        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth,clientId);

        Assert.Equal(false, result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Invalid_Email_Format()
    {
        int clientId = 1;
        string firstName = "Jan";
        string lastName = "Kowalski";
        string email = "invalidemail"; // invalid email format
        var dateOfBirth = new DateTime(1990, 1, 1);
        var userService = new UserService();
        
        bool result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        
        Assert.Equal(false,result);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Invalid()
    {
        var clientId = 1;
        var firstName = "John";
        var lastName = "Doe";
        var email = "not-an-email";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var userService = new UserService();
        
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        
        Assert.False(result);
    }
    
    [Fact]
    public void AddUser_ShouldReturnFalse_WhenUnderage21()
    {
        var clientId = 1;
        var firstName = "Filip";
        var lastName = "Kłopot";
        var email = "Filip.Kłopot@example.com";
        var dateOfBirth = DateTime.Now.AddYears(-20);
        var userService = new UserService();
        
        var result = userService.AddUser(firstName, lastName, email, dateOfBirth, clientId);
        
        Assert.False(result);
    }
}

