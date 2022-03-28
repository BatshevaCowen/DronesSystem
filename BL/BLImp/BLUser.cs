using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BL
{
    internal sealed partial class BL : IBL
    {
        /// <summary>
        /// Copy DO.User to BO.User
        /// </summary>
        /// <param name="userDO">DO.User</param>
        /// <returns>BO.User</returns>
        User UserDoBOAdapter(DO.User userDO)
        {
            User userBO = new User();
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }
        public void AddUser(User tmpUser)
        {
            // Instance.AddUser(tmpUser);
            DO.User u = new()
            {
                Password = tmpUser.Password,
                UserName = tmpUser.UserName,
                Permission=DO.Permit.User,
                MyActivity=DO.Activity.On
            };
            dalo.AddUser(u);
        }
        /// <summary>
        /// Returns User that has that name
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            DO.User userDO;
            try
            {
                userDO = dalo.GetUser(userName);
                return UserDoBOAdapter(userDO);
            }
            catch (DO.BadUserException e)
            {
                throw new BOBadUserException(e.Message, userName);
            }
        }
    }
}
