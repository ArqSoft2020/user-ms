using Microsoft.EntityFrameworkCore;
using userService.Repository;
using userService.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using userService.Model;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

using Novell.Directory.Ldap;

using Microsoft.Extensions.Configuration;  

namespace userService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _dbContext;
        private readonly string _ip;
        private readonly string _port;
        private readonly string _BaseDN;

        public UserRepository(UserContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _ip = config.GetSection("LDAP").GetSection("IP_Address").Value;
            _port = config.GetSection("LDAP").GetSection("port").Value;
            _BaseDN = config.GetSection("LDAP").GetSection("BaseDN").Value;
        }

        public void DeleteUser(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            _dbContext.Users.Remove(user);
            Save();
        }

        public User GetUserById(int userId)
        {
            return _dbContext.Users.Find(userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void InsertUser(User user)
        {
            user.passhash_user = CalculateHash(user.passhash_user);
            _dbContext.Add(user);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user, int userId)
        {
            User userToUpdate = _dbContext.Users.Find(userId);

            if (user.username_user != userToUpdate.username_user && user.username_user != null){
                userToUpdate.username_user = user.username_user; 
            }

            if (user.passhash_user != userToUpdate.passhash_user && user.passhash_user != null){
                user.passhash_user = CalculateHash(user.passhash_user);
                userToUpdate.passhash_user = user.passhash_user;
            } 

            if (user.address_user != userToUpdate.address_user && user.address_user != null){
                userToUpdate.address_user = user.address_user;
            }

            if (user.cellphone_user != userToUpdate.cellphone_user && user.cellphone_user != null){
                userToUpdate.cellphone_user = user.cellphone_user;
            }

            if (user.email_user != userToUpdate.email_user && user.email_user != null){
                userToUpdate.email_user = user.email_user;
            }


            _dbContext.Entry(userToUpdate).State = EntityState.Modified;
            Save();
        }

        
        public User GetUserByEmail(string email){
            User userGot = _dbContext.Users.SingleOrDefault(
                a => a.email_user.Equals(email)
            );
            return userGot;
        }
        

        public int CountUserByEmail(string email){
            var userGot = _dbContext.Users.Where(
                a => a.email_user.Equals(email)
            ).Count();
            return userGot;
        }


        public string CalculateHash(string input)
        {
            var salt = GenerateSalt(16);
            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);
            return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
        }

        public byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];
            using (var random = RandomNumberGenerator.Create())
            {
        	    random.GetBytes(salt);
            }
            return salt;
        }

        public bool CheckMatch(string hash, string input)
        {
            try
            {
        	    var parts = hash.Split(':');
        	    var salt = Convert.FromBase64String(parts[0]);
        	    var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);
        	    return parts[1].Equals(Convert.ToBase64String(bytes));
            }
            catch
            {
        	    return false;
            }            
        }

        public bool CheckLDAP(string userDN, string userPassword)
        {
            try
            {
                using (var conn = new LdapConnection())
                {
                    conn.Connect(_ip, Int32.Parse(_port));
                    conn.Bind("cn=" + userDN + _BaseDN, userPassword);
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}