using BookStoreTutorial.Models.DataLayer.SeedData;
using BookStoreTutorial.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataLayer
{
    public class BookstoreContext : IdentityDbContext<User>
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //For IdentityDbContext
            base.OnModelCreating(modelBuilder);

            //Create many-to-many relationship for BookAuthor with Fluent API
            //Create composite key
            modelBuilder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });

            //Create foregin key. Each book can have many authors
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId);

            //Create foregin key. Each author can have many books
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Author)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId);

            //Create one-to-many for Book and Genre
            modelBuilder.Entity<Book>().HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .OnDelete(DeleteBehavior.Restrict);     //disable cascading deletion - can't delete genre if books with that genre exist

            //Seed model
            modelBuilder.ApplyConfiguration(new SeedGenres());
            modelBuilder.ApplyConfiguration(new SeedBooks());
            modelBuilder.ApplyConfiguration(new SeedAuthors());
            modelBuilder.ApplyConfiguration(new SeedBookAuthors());
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            //Create an admin and role for the first time
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "Password@2020";
            string roleName = "Admin";

            //Create role if it doesn't exist
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            //Create the user if it doesn't exist
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
