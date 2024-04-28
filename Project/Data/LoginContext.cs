    using Microsoft.EntityFrameworkCore;
    using Project.Models;

    namespace Project.Data
    {
        public class LoginContext : DbContext
        {
            public LoginContext(DbContextOptions<LoginContext> options) : base(options) { }

            // DbSet represents a collection of entities in the context
            public DbSet<LoginModel> dblog { get; set; }
             public DbSet<PersonModel> Persons{ get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<LoginModel>().ToTable("dblog"); // Ensure correct table name
        //                                                        // Additional configurations...
        //}
    }

    }
