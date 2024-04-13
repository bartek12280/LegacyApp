using System;

namespace LegacyApp
{
    public class UserService(
        ClientRepositoryInterface clientRepositoryInterface,
        UserCreditServiceInterface userCreditServiceInterface)
    {
        private readonly ClientRepositoryInterface _clientRepositoryInterface = clientRepositoryInterface;
        private readonly UserCreditServiceInterface _userCreditServiceInterface = userCreditServiceInterface;

        private const string ImportantClient = "ImportantClient";
        private const string VeryImportantClient = "VeryImportantClient";

        public UserService() : this(new ClientRepository(), new UserCreditService())
        {
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var client = _clientRepositoryInterface.GetById(clientId);
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            if (!ValidateUser(user))
            {
                return false;
            }

            if (IsClientVeryImportant(client)) user.HasCreditLimit = false;
            else if (IsClientImportant(client))
            {
                user.CreditLimit = CountCreditLimitForImportantClient(user);
            }
            else
            {
                user.CreditLimit = CountCreditLimitForClient(user);
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        private bool IsNameEmpty(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return true;
            }

            return false;
        }

        private bool IsEmailInvalid(string email)
        {
            if (!email.Contains("@") && !email.Contains(".")) return true;
            return false;
        }

        private bool IsAgeLowerThan21(int age)
        {
            if (age < 21) return true;
            return false;
        }

        private int CountAge(DateTime dateOfBirth)
        {
            var now = DateTime.Today;
            int age = now.Year - dateOfBirth.Year;
            if (now < dateOfBirth.AddYears(age)) age--;
            return age;
        }

        private bool ValidateUser(User user)
        {
            return !IsNameEmpty(user.FirstName, user.LastName) && !IsEmailInvalid(user.EmailAddress) &&
                   !IsAgeLowerThan21(CountAge(user.DateOfBirth));
        }

        private bool IsClientVeryImportant(Client client)
        {
            return client.Type == VeryImportantClient;
        }

        private bool IsClientImportant(Client client)
        {
            return client.Type == ImportantClient;
        }

        private int CountCreditLimitForImportantClient(User user)
        {
            int creditLimit = _userCreditServiceInterface.GetCreditLimit(user.LastName, user.DateOfBirth);
            creditLimit *= 2;
            return creditLimit;
        }

        private int CountCreditLimitForClient(User user)
        {
            int creditLimit = _userCreditServiceInterface.GetCreditLimit(user.LastName, user.DateOfBirth);
            return creditLimit;
        }
    }
}