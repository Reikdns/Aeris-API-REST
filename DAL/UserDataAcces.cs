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

    public void SaveUserPersonalData(User user){

        using(var command =  _connection.CreateCommand()){
            command.CommandText = @"INSERT INTO USERS (nombres, apellidos, edad, rol, identificacion)" 
            + "VALUES (@nombres, @apellidos, @edad, @rol, @identificacion)";
            command.Parameters.AddWithValue("@nombres", user.Nombres);
            command.Parameters.AddWithValue("@apellidos", user.Apellidos);
            command.Parameters.AddWithValue("@edad", user.Edad);
            command.Parameters.AddWithValue("@rol", user.Rol);
            command.Parameters.AddWithValue("@identificacion", user.Identificacion);
            command.ExecuteNonQuery();
        }
    }

    public void SaveDefaultUser(DefaultUser user)
    {
        using(var command = _connection.CreateCommand())
        {
            SqlTransaction transaction = _connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                SaveUser(user, command);
                int userId = GetUserKey(user, command);
                SaveLoginUser(user, command, userId);
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }      
        }
    }

    private static void SaveLoginUser(DefaultUser user, SqlCommand command, int userId)
    {
        command.CommandText = @"INSERT INTO LoginUser(user_id, email, password, salt)"
                            + "VALUES (@user_id, @email, @password, @salt)";
        command.Parameters.AddWithValue("@user_id", userId);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@password", user.Password);
        command.Parameters.AddWithValue("@salt", user.Salt);
        command.ExecuteNonQuery();
    }

    private void SaveUser(DefaultUser user, SqlCommand command)
    {
        command.CommandText = @"INSERT INTO USERS (email, rol)"
                            + "VALUES (@email, @rol)";
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@rol", "Admin");
        command.ExecuteNonQuery();
        command.Parameters.Clear();
    }

    private static int GetUserKey(DefaultUser user, SqlCommand command)
    {
        command.CommandText = $"SELECT user_id FROM Users WHERE email = @email";
        command.Parameters.AddWithValue("@email", user.Email);

        SqlDataReader dataReader = command.ExecuteReader();
        command.Parameters.Clear();

        dataReader.Read();
        int userId = (int)dataReader["user_id"];
        dataReader.Close();
        return userId;
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
        User user = Select(key, value);
        return user;
    }

    public DefaultUser DefaultSearchByKey(string key, string value)
    {
        DefaultUser user = DefaultSelect(key, value);
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

    private DefaultUser DefaultSelect(string key, string value)
    {
        SqlDataReader dataReader;
        DefaultUser user = new DefaultUser();

        using (var command = _connection.CreateCommand())
        {
            command.CommandText = $@"SELECT * FROM LoginUser WHERE @value = {key}";
            command.Parameters.AddWithValue("@value", value);
            dataReader = command.ExecuteReader();
            if (dataReader.HasRows)
            {
                dataReader.Read();
                user = DefaultDataMapInReader(dataReader);
            }
            return user;
        }
    }

    private User DataMapInReader (SqlDataReader dataReader) {
        if (!dataReader.HasRows) return null;
        User user = new User ( );
        user.UserId = (int) dataReader["user_id"];
        user.Nombres =  ColumnValueIsNull(dataReader["nombres"]) ? null : (string) dataReader["nombres"];
        user.Apellidos = ColumnValueIsNull(dataReader["apellidos"]) ? null : (string) dataReader["apellidos"];
        user.Email = (string) dataReader["email"];
        user.Edad = ColumnValueIsNull(dataReader["edad"]) ? null : (int) dataReader["edad"];             
        user.Rol = (string) dataReader["rol"];
        user.Identificacion = ColumnValueIsNull(dataReader["identificacion"]) ? null : (string) dataReader["identificacion"];;
        return user;
    }

    private bool ColumnValueIsNull(object reader)
    {
        return DBNull.Value.Equals(reader);
    }

    private DefaultUser DefaultDataMapInReader(SqlDataReader dataReader)
    {
        if (!dataReader.HasRows) return null;
        DefaultUser user = new DefaultUser();
        user.UserId = (int)dataReader["user_id"];
        user.Email = (string)dataReader["email"];
        user.Password = (string)dataReader["password"];
        user.Salt = (string)dataReader["salt"];

        return user;
    }
}