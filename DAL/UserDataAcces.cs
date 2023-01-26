namespace DAL;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Entity;

public class UserDataAcces{

    private readonly SqlConnection _connection;
    
    public UserDataAcces(ConnectionManager connection){
        _connection = connection._connection;
    }

    public void SaveUser(User user){

        using(var command =  _connection.CreateCommand()){
            command.CommandText = @"INSERT INTO USERS (nombres, apellidos, edad, username, password, rol, identificacion, salt)" 
            + "VALUES (@nombres, @apellidos, @edad, @username, @password, @rol, @identificacion, @salt)";
            command.Parameters.AddWithValue("@nombres", user.Nombres);
            command.Parameters.AddWithValue("@apellidos", user.Apellidos);
            command.Parameters.AddWithValue("@edad", user.Edad);
            command.Parameters.AddWithValue("@username", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@rol", user.Rol);
            command.Parameters.AddWithValue("@identificacion", user.Identificacion);
            command.Parameters.AddWithValue("@salt", user.Salt);
            command.ExecuteNonQuery();
        }
    }

    public void SaveDefaultUser(DefaultUser user)
    {
        using(var command = _connection.CreateCommand())
        {
            command.CommandText = @"INSERT INTO LoginUser(email, password, salt)"
            + "VALUES (@email, @password, @salt)";
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@salt", user.Salt);
            command.ExecuteNonQuery();
        }
    }

    public List<User> SearchAll ( ) {
        SqlDataReader dataReader;
        List<User> users = new List<User> ( );
        using (var command = _connection.CreateCommand ( )) {
            command.CommandText = "SELECT * FROM Users";
            dataReader = command.ExecuteReader ( );
            if (dataReader.HasRows) {
                while (dataReader.Read ( )) {
                    User user = DataMapInReader (dataReader);
                    users.Add (user);
                }
            }
        }
        return users;
    }

    public User SearchByKey(string key, string value)
    {
        SqlDataReader dataReader;
        User user = Select(key, value);
        return user;
    } 

    private User Select(string key, string value)
    {
        SqlDataReader dataReader;
        User user = new User();

        using(var command = _connection.CreateCommand())
        {
            command.CommandText = $@"SELECT * FROM Users WHERE @value = {key}";
            command.Parameters.AddWithValue("@value", value);
            dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                dataReader.Read();
                user = DataMapInReader(dataReader);
            }
            return user;
        }
    } 

    private User DataMapInReader (SqlDataReader dataReader) {
        if (!dataReader.HasRows) return null;
        User user = new User ( );
        user.UserId = (int) dataReader["user_id"];
        user.Nombres = (string) dataReader["nombres"];
        user.Apellidos = (string) dataReader["apellidos"];
        user.Edad = (int) dataReader["edad"];
        user.Username = (string) dataReader["username"];
        user.Password = (string) dataReader["password"];                     
        user.Rol = (string) dataReader["rol"];
        user.Identificacion = (string) dataReader["identificacion"];
        user.Salt = (string)dataReader["salt"];

        return user;
    }
}