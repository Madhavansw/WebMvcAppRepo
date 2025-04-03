using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebMvcApp.Models;

namespace WebMvcApp.Controllers;

public class EmployeeController : Controller
{
    private readonly string _connectionString;

    public EmployeeController(IConfiguration configuration)
    {
        // Retrieve the connection string from the configuration
      this. _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

     public async Task<IActionResult> IndexTwo()
    {
        using var con = new SqlConnection(this._connectionString);
        var emp = await con.QueryAsync<Employee>("select * from Employees");
        return View(emp);
    }
    public async Task<IActionResult> Index()
    {
        using var connection = new SqlConnection(_connectionString);
        var employees = await connection.QueryAsync<Employee>("SELECT * FROM Employees");
        return View(employees);
    }

    public async Task<IActionResult> GetEmployeeTable()
    {
        using var connection = new SqlConnection(_connectionString);
        var employees = await connection.QueryAsync<Employee>("SELECT * FROM Employees");
        return PartialView("_EmployeeTable", employees);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var employee = await connection.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employees WHERE Id = @Id", new { Id = id });
        return PartialView("_Edit", employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Employee employee)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("UPDATE Employees SET Name = @Name, Position = @Position, Office = @Office, Age = @Age WHERE Id = @Id", employee);
        return Json(new { success = true });
    }
}
