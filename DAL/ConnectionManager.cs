namespace DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
public class ConnectionManager{

    internal SqlConnection _connection;
    public ConnectionManager (string connectionString) {
        _connection = new SqlConnection (connectionString);
    }
    public void Open ( ) {
        _connection.Open ( );
    }
    public void Close ( ) {
        _connection.Close ( );
    }
}
