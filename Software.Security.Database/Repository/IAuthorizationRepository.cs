﻿using Software.Security.Database.Models;
using System.Collections.Generic;

namespace Software.Security.Database.Repository

{
    public interface IAuthorizationRepository
    {
        bool IsUserExist(string login, string password);
        bool Register(string login, string password);
        User GetUser(string login);
        IEnumerable<User> GetUsers();
        bool IsUserOwnerMessage(int userId, int messageId);
        bool IsUserAllowedToEdit(int userId, int messageId);
        bool RemoveUserFromAllowedMessage(int userId, int messageId);
        void AddUserToAllowedMessage(int userId, int messageId);
        IEnumerable<User> GetUsersFromAllowedMessage(int messageId);
    }
}