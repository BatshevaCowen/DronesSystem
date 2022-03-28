using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace DAL
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// add user
        /// throw BadUserException
        /// </summary>
        /// <param name="tmpUser">user to add</param>
        public void AddUser(User tmpUser)
        {
            if (DataSource.userList.FirstOrDefault(user => user.UserName == tmpUser.UserName && user.MyActivity == Activity.On) != null)
                throw new BadUserException("User already exist", tmpUser.UserName);
            DataSource.userList.Add(tmpUser.Clone());
        }
        /// <summary>
        /// get solid user by his name
        /// throw BadUserException
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
                return myUser.Clone();
            throw new BadUserException("User doesn't exist", userName);
        }
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns>IEnumerable implemented by users</returns>
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.userList
                   where user.MyActivity == Activity.On
                   select user.Clone();
        }
        /// <summary>
        /// get all users that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by users satisfies the cindition</returns>
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            IEnumerable<User> myUsers = from user in DataSource.userList
                                        where user.MyActivity == Activity.On
                                        where predicate(user)
                                        select user.Clone();
            if (myUsers == null)
                throw new ReadDataException("No User meets the conditions");
            return myUsers;
        }

        /// <summary>
        /// update user (delete the old and add the new)
        /// throw BadUserException
        /// </summary>
        /// <param name="userToUpdate">user To Update</param>
        public void UpdateUser(User userToUpdate)
        {
            User tmpUser = DataSource.userList.FirstOrDefault(user => user.UserName == userToUpdate.UserName && user.MyActivity == Activity.On);
            if (tmpUser == null)
                throw new BadUserException("User doesn't exist", userToUpdate.UserName);
            DeleteUser(tmpUser.UserName);
            AddUser(userToUpdate);
        }
        /// <summary>
        /// delete user
        /// throw BadUserException
        /// </summary>
        /// <param name="userName">name of user to delete</param>
        public void DeleteUser(string userName)
        {
            User myUser = DataSource.userList.FirstOrDefault(user => user.UserName == userName && user.MyActivity == Activity.On);
            if (myUser != null)
                myUser.MyActivity = Activity.Off;
            else throw new BadUserException("User doesn't exist", userName);
        }
    }
}
