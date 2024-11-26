using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class UserService
    {
        private UserRepository userRepository;
        public UserService()
        {
            userRepository = new UserRepository("myDatabase.sqlite");
        }
        public void AddUser(User user)
        {
            User _user = userRepository.SearchUser(user.MembershipId);
            if (user.MembershipId == _user.MembershipId)
            {
                Console.WriteLine("User already exists with this Membership ID!");
                return;
            }
            userRepository.AddUser(user);
        }
        public void ListUsers()
        {
            User[] users = userRepository.SearchUsers();
            for (int i = 0; i < users.Length; i++)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Name: " + users[i].Name);
                Console.WriteLine("Membership ID: " + users[i].MembershipId);
                Console.WriteLine();
            }
        }
        public User SearchUser(int membershipId)
        {
            User user = userRepository.SearchUser(membershipId);
            return user;
        }
    }
}