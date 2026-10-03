using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagementApi.Models;

namespace UserManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private static readonly Dictionary<int, User> Users = new();
    private static readonly object SyncRoot = new();
    private static int nextId;

    [HttpGet]
    public ActionResult<IReadOnlyCollection<User>> GetAll()
    {
        lock (SyncRoot)
        {
            return Ok(Users.Values.OrderBy(user => user.Id).ToArray());
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        lock (SyncRoot)
        {
            return Users.TryGetValue(id, out var user) ? Ok(user) : NotFound();
        }
    }

    [HttpPost]
    public ActionResult<User> Create(UserRequest request)
    {
        lock (SyncRoot)
        {
            if (EmailIsTaken(request.Email))
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            var user = new User(
                Interlocked.Increment(ref nextId),
                request.Name.Trim(),
                request.Email.Trim(),
                request.Age);
            Users.Add(user.Id, user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UserRequest request)
    {
        lock (SyncRoot)
        {
            if (!Users.ContainsKey(id))
            {
                return NotFound();
            }

            if (EmailIsTaken(request.Email, id))
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            Users[id] = new User(id, request.Name.Trim(), request.Email.Trim(), request.Age);
            return NoContent();
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        lock (SyncRoot)
        {
            return Users.Remove(id) ? NoContent() : NotFound();
        }
    }

    private static bool EmailIsTaken(string email, int? exceptId = null)
    {
        return Users.Values.Any(user =>
            user.Id != exceptId &&
            string.Equals(user.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}