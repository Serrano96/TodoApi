using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Model;

namespace TodoApi
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> Items { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options)
        : base(options) { }
    }
}
