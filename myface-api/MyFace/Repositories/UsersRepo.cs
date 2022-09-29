using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFace.Helpers.PasswordHelper;
using MyFace.Models.Database;
using MyFace.Models.Request;
using MyFace.Helpers.PasswordHelper;

namespace MyFace.Repositories
{
    public interface IUsersRepo
    {
        IEnumerable<User> Search(UserSearchRequest search);
        int Count(UserSearchRequest search);
        User GetByUserName(string userName);
        User GetById(int id);
        bool IsValidAccount (string authorization);

        User Create(CreateUserRequest newUser);
        User Update(int id, UpdateUserRequest update);
        void Delete(int id);
    }
    
    public class UsersRepo : IUsersRepo
    {
        private readonly MyFaceDbContext _context;

        public UsersRepo(MyFaceDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<User> Search(UserSearchRequest search)
        {
            return _context.Users
                .Where(p => search.Search == null || 
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ))
                .OrderBy(u => u.Username)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(UserSearchRequest search)
        {
            return _context.Users
                .Count(p => search.Search == null || 
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ));
        }

        public User GetById(int id)
        {
            return _context.Users
                .Single(user => user.Id == id);
        }

        public User GetByUserName(string userName)
        {
            return _context.Users
                .Single(user => user.Username == userName);
        }

        public bool IsValidAccount (string authorization)
        {
            string encodedData = Encoding.UTF8.GetString(Convert.FromBase64String(authorization.Substring("Base ".Length)));
            string[] userNamePassword = encodedData.Split(":");
            string userName = userNamePassword[0];
            string password = userNamePassword[1];

            User user = _context.Users
                .Single(user => user.Username == userName);
            string hashedPassword = PasswordHelper.GetHash(password, user.Salt);


            if(user.Username != userName || user.HashedPassword != hashedPassword)
            {
                return false;
            }

            return true;
        }
        public User Create(CreateUserRequest newUser)
        {
            byte[] salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.GetHash(newUser.Password, salt);
            var insertResponse = _context.Users.Add(new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Username = newUser.Username,
                HashedPassword = hashedPassword,
                Salt = salt,
                ProfileImageUrl = newUser.ProfileImageUrl,
                CoverImageUrl = newUser.CoverImageUrl,
            });
            _context.SaveChanges();

            return insertResponse.Entity;
        }

        public User Update(int id, UpdateUserRequest update)
        {
            var user = GetById(id);

            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.Username = update.Username;
            user.Email = update.Email;
            user.ProfileImageUrl = update.ProfileImageUrl;
            user.CoverImageUrl = update.CoverImageUrl;

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}