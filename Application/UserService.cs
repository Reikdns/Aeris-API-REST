namespace BLL;

using System;
using System.Collections.Generic;
using DAL;
using Entity;
public class UserService
{
     private readonly ConnectionManager _connection;
    private readonly UserDataAcces _repository;
    public UserService(string cadenaDeConexión)
    {
        _connection = new ConnectionManager(cadenaDeConexión);
        _repository = new UserDataAcces(_connection);
    }

     public SaveUserResponse SaveUser(User user)
    {
        if(Validate(user.Identificacion)){        
            try
            {                                          
                _connection.Open();                
                _repository.SaveUser(user);
                _connection.Close();
                return new SaveUserResponse(user);
            }
            catch (Exception e)
            {
                return new SaveUserResponse(e.Message);
            }
        }
        else {
            return new SaveUserResponse("El usuario ya ha sido registrado.");
        }
    }

    public bool Validate(string identificacion){
        if (SearchById(identificacion).Error)
        {  
            return true;
        } 
        return false;
    }

    public SearchByIdResponse SearchById(string identificacion)
    {           
        var response = SearchAllUsers();
        foreach (var user in response.Users)
        {
            if(user.Identificacion == identificacion){
                return new SearchByIdResponse(user);
            }             
        }
        return new SearchByIdResponse("El usuario no ha sido encontrado.");
    }            
    
    public SearchUsersResponse SearchAllUsers()
    {
        try
        {
            _connection.Open();
            List<User> Users = _repository.SearchAll();
            _connection.Close();
            return new SearchUsersResponse(Users);
        }
        catch (Exception e)
        {
            return new SearchUsersResponse(e.Message);
        }
    }
}

public class SearchByIdResponse
{
    public bool Error { get; set; }
    public string Message { get; set; }
    public User User { get; set; }
    public SearchByIdResponse(User user)
    {
        Error = false;
        User = user;
    }
    public SearchByIdResponse(string message)
    {
        Error = true;
        Message = message;
    }
}

public class SearchUsersResponse
{
    public bool Error { get; set; }
    public string Message { get; set; }
    public List<User> Users = new List<User>();
    public SearchUsersResponse(List<User> users)
    {
        Error = false;
        Users = users;
    }
    public SearchUsersResponse(string message)
    {
        Error = true;
        Message = message;
    }
}

public class SaveUserResponse
{
    public bool Error { get; set; }
    public string Message { get; set; }
    public User User { get; set; }
    public SaveUserResponse(User user)
    {
        Error = false;
        User = user;
    }
    public SaveUserResponse(string message)
    {
        Error = true;
        Message = message;
    }
}