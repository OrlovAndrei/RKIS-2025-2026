using Microsoft.EntityFrameworkCore;

namespace TodoList.Interfaces;

public interface IConnectionStrategy
{
    void Configure(DbContextOptionsBuilder optionsBuilder);
}