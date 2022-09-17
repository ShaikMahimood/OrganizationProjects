using OrganizationRepository.Exceptions;
using OrganizationRepository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OrganizationRepository.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;

        public UserServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("databaseconnection"));
           
        }
        public bool UserLogin(Credentials login)
        {
            bool IsLogin = false;
            try
            {
                using (_command = new SqlCommand("UserLoginProc", _connection))
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                        _command.CommandType = CommandType.StoredProcedure;
                        _command.Parameters.AddWithValue("@EmailId", login.EmailId);
                    }

                    _reader = _command.ExecuteReader();
                    while (_reader.Read())
                    {
                        if (login.Password.Equals(_reader["Password"]))
                        {
                            IsLogin = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string logMessage = ex.Message + " stack trace " + ex.StackTrace;

                if (ex.InnerException.Message != null)
                    logMessage = logMessage + " inner message " + ex.InnerException.Message;
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return IsLogin;
        }
        public bool AddNewUser(UserRegistration user)
        {
            bool IsAdded = false;
            var GetData = GetUserByUsernameOrEmailId(user.EmailId);

            try
            {
                if (GetData.Count() == 0)
                {
                    using (_command = new SqlCommand("UserInsertProc", _connection))
                    {
                        if (_connection.State == ConnectionState.Closed)
                            _connection.Open();
                        _command.CommandType = CommandType.StoredProcedure;

                        _command.Parameters.AddWithValue("@UserName", user.UserName);
                        _command.Parameters.AddWithValue("@EmailId", user.EmailId);
                        _command.Parameters.AddWithValue("@Password", user.Password);
                        _command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        _command.Parameters.AddWithValue("@Address", user.Address);


                        int i = _command.ExecuteNonQuery();

                        if (i == 1)
                            IsAdded = true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            
            return IsAdded;
        }

        public IEnumerable<UserData> GetUsers()
        {
            List<UserData> GetData = new List<UserData>();
            try
            {
                using (_command = new SqlCommand("UserSelectProc", _connection))
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }

                    _reader = _command.ExecuteReader();
                    while (_reader.Read())
                    {
                        UserData userData = new UserData()
                        {
                            UserId = _reader.GetInt32(0),
                            Username = _reader.GetString(1),
                            EmailId = _reader.GetString(2),
                            PhoneNumber = _reader.GetInt64(3),
                            Address = _reader.GetString(4)
                        };
                        GetData.Add(userData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return GetData;
        }
        public IEnumerable<UserData> GetUserByUsernameOrEmailId(string UsernameOrEmailId)
        {
            List<UserData> GetData = new List<UserData>();
            try
            {
                using (_command = new SqlCommand("UserGetByNameOrEmailIdProc", _connection))
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();
                    
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@UsernameOrEmailId", UsernameOrEmailId);
                    _reader = _command.ExecuteReader();

                    while (_reader.Read())
                    {
                        UserData userData = new UserData()
                        {
                            UserId = _reader.GetInt32(0),
                            Username = _reader.GetString(1),
                            EmailId = _reader.GetString(2),
                            PhoneNumber = _reader.GetInt64(3),
                            Address = _reader.GetString(4)
                        };
                        GetData.Add(userData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return GetData;
        }
        public bool UpdateUser(UserRegistration user)
        {
            bool IsUpdated = false;
            var GetData = GetUserByUsernameOrEmailId(user.EmailId);

            try
            {
                if (GetData.Count() > 0)
                {
                    using (_command = new SqlCommand("UserUpdateProc", _connection))
                    {
                        if (_connection.State == ConnectionState.Closed)
                            _connection.Open();

                        _command.CommandType = CommandType.StoredProcedure;

                        _command.Parameters.AddWithValue("@UserName", user.UserName);
                        _command.Parameters.AddWithValue("@EmailId", user.EmailId);
                        _command.Parameters.AddWithValue("@Password", user.Password);
                        _command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        _command.Parameters.AddWithValue("@Address", user.Address);

                        int i = _command.ExecuteNonQuery();

                        if (i == 1)
                            IsUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return IsUpdated;
        }
        public bool DeleteUserByEmailId(string EmailId)
        {
            bool IsDeleted = false;
            try
            {
                using (_command = new SqlCommand("UserDeleteProc", _connection))
                {
                    if (_connection.State == ConnectionState.Closed)
                        _connection.Open();

                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@EmailId", EmailId);

                    int i = _command.ExecuteNonQuery();
                    if (i == 1)
                        IsDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw new UserExceptions(ex.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
            return IsDeleted;
        }

    }
}
