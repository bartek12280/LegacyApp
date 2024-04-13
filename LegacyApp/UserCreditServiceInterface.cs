using System;

namespace LegacyApp;

public interface UserCreditServiceInterface
{
    int GetCreditLimit(string lastName, DateTime dateOfBirth);
}