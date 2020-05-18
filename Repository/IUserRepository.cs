using userService.Model;
using System.Collections.Generic;

namespace userService.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int userId);
        void InsertUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user, int userId);
        User GetUserByEmail(string email);
        int CountUserByEmail(string email);
        void Save();
        string CalculateHash(string input);
        byte[] GenerateSalt(int length);
        bool CheckMatch(string hash, string input);
        bool CheckLDAP(string userDN, string userPassword);
    }
}