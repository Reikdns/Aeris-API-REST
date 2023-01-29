namespace BLL;

using System;
using System.Collections.Generic;
using DAL;
using Entity;
using System.Data.SqlClient;
public class UserService
{
    private readonly ConnectionManager _connection;
    private readonly UserDataAcces _repository;
    public UserService(string cadenaDeConexión)
    {
        _connection = new ConnectionManager(cadenaDeConexión);
        _repository = new UserDataAcces(_connection);
    }

    public RequestResponse<User> SaveUserPersonalData(User user)
    {
        if(Validate("identificacion", user.Identificacion) && Validate("email", user.Email)){        
            try
            {                                          
                _connection.Open();                
                _repository.SaveUserPersonalData(user);
                _connection.Close();
                return new RequestResponse<User>(user);
            }
            catch (Exception e)
            {
                return new RequestResponse<User>(e.Message);
            }
        }
        else {
            return new RequestResponse<User>("El usuario ya ha sido registrado.");
        }
    }

    public RequestResponse<DefaultUser> SaveUser(DefaultUser user)
    {
        if(DefaultValidate("email", user.Email))
        {
            try
            {
                _connection.Open();
                _repository.SaveDefaultUser(user);
                _connection.Close();
                return new RequestResponse<DefaultUser>("La cuenta ha sido registrada exitosamente.");
            }
            catch (Exception e)
            {
                return new RequestResponse<DefaultUser>(e.Message);
            }
        }
        else
        {
            return new RequestResponse<DefaultUser>("El correo ingresado ya está asociado a una cuenta.");
        }
    }

    private bool Validate(string key, string value){

        var response = SearchByKey(key, value);

        if (response.Error)
        {  
            return true;
        } 
        return false;
    }

    private bool DefaultValidate(string key, string value)
    {
        var response = DefaultSearchByKey(key, value);

        if (response.Error)
        {
            return true;
        }
        return false;
    }
    
    public RequestResponse<List<User>> SearchAllUsers()
    {
        try
        {
            _connection.Open();
            List<User> Users = _repository.SearchAll();
            _connection.Close();
            return new RequestResponse<List<User>>(Users);
        }
        catch (Exception e)
        {
            return new RequestResponse<List<User>>(e.Message);
        }
    }

    public RequestResponse<User> SearchByKey(string key, string value)
    {
        try
        {
            _connection.Open();
            User user = _repository.SearchByKey(key, value);
            _connection.Close();

            if(user.Email == null)
            {
                throw new Exception("El usuario no ha sido encontrado");
            }

            return new RequestResponse<User>(user);
        }
        catch (Exception e)
        {
            return new RequestResponse<User>(e.Message);
        }
    }

    public RequestResponse<DefaultUser> DefaultSearchByKey(string key, string value)
    {
        try
        {
            _connection.Open();
            DefaultUser user = _repository.DefaultSearchByKey(key, value);
            _connection.Close();

            if (user.Email == null)
            {
                throw new Exception("El usuario no ha sido encontrado");
            }

            return new RequestResponse<DefaultUser>(user);
        }
        catch (Exception e)
        {
            return new RequestResponse<DefaultUser>(e.Message);
        }
    }

}

public class RequestResponse<T>
{
    public bool Error { get; set; }
    public string Message { get; set; }
    public T Response { get; set; }
    public RequestResponse(T response)
    {
        Error = false;
        Response = response;
    }
    public RequestResponse(string message)
    {
        Error = true;
        Message = message;
    }
}
