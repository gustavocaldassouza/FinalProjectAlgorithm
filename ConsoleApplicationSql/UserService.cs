using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApplicationSql
{
    public class UserService
    {
        User[] users = new User[2];

        public void AddUser(User user)
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].MembershipId == user.MembershipId)
                {
                    Console.WriteLine("User already exists with this membership ID!");
                    Console.WriteLine("Use another membership ID!");
                    return;
                }
                if (users[i] == null)
                {
                    users[i] = user;
                    return;
                }
            }

            // initialize a new array with new length and transfer the old one to the new one.
            User[] newArrUsers = new User[users.Length + 5];
            for (int i = 0; i < users.Length; i++)
            {
                newArrUsers[i] = users[i];
            }
            newArrUsers[users.Length] = user;
            users = newArrUsers;
        }
        public void ListUsers()
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i] == null) break;
                Console.WriteLine("Name: " + users[i].Name);
                Console.WriteLine("Membership ID: " + users[i].MembershipId);
                Console.WriteLine();
            }
        }
        public User SearchUser(int membershipId)
        {
            foreach (var user in users)
            {
                if (user == null) break;
                if (user.MembershipId == membershipId)
                {
                    return user;
                }
            }
            return new User();
        }
    }
}