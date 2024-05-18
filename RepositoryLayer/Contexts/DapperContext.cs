﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace RepositoryLayer.Contexts;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("FundooNotesConnection");
    }
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
