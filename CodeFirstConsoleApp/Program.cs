using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstConsoleApp
{
    class Program
    {
        /*
         * [NOTE ON MODEL CHANGES]---
         * In order to make changes to the data model,
         * there needs to be changes to the database
         * model. Migrations are used to apply these
         * types of changes.
         * 
         * Open Package Manager and use the command
         * 'Enable-Migrations'. This will add a config
         * folder, which allows for changes to the 
         * folder where migrations are kept, register
         * providers to third party databases, and 
         * specify seed data. The InitialCreatMigration
         * folder is where the code for the database
         * change is.
         * 
         * Once changes to the domain model are made
         * run the 'Add-Migration [name]' command. 
         * The [name] argument is optional. Then run
         * the 'Update-Database' command to apply the
         * changes to the database.
         */
        static void Main(string[] args)
        {
            // The using statement frees resources 
            // when they're not in use. BlogContext
            // opens a session with the database 
            // and allows saving and querying.
            // BlogContext() is defined below.
            using(var db = new BlogContext())
            {
                Console.WriteLine("What is the name of your new  blog?");
                string name = Console.ReadLine();
                Blog blog = new Blog() { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();
                // LINQ queries can be run to find
                // and list items in the db
                IEnumerable<Blog> query = from b in db.Blogs
                            orderby b.Name
                            select b;
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }



        // Each property is mapped to a column in 
        // the Blog table
        public class Blog
        {
            // The Id property is what the code first
            // convention expects as a primary key by
            // default
            public int BlogId { get; set; }
            public string Name { get; set; }
            // The virtual modifier will allow for 
            // Lazy Loading, which means EF will query
            // and populate the contents of those 
            // properties automatically whenever they
            // are accessed. So in other words, the
            // posts in the database will be queried
            // or updated whenever the blog is.
            public virtual List<Post> Posts { get; set; }
        }



        // [REPEATED INFO]---
        // Each property is mapped to a column in the
        // Post table
        public class Post
        {
            // The Id property is what the code first
            // convention expects as a primary key by
            // default
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            // [REPEATED INFO]---
            // The virtual modifier will allow for 
            // Lazy Loading, which means EF will query
            // and populate the contents of those 
            // properties automatically whenever they
            // are accessed. So in other words, the
            // blog in the database will be queried
            // or updated whenever the post is.
            public int BlogId { get; set; }
            public virtual Blog Blog { get; set; }
        }



        // This class was added using Migrations
        public class User
        {
            /*
             * [NOTE ON DATA ANNOTATIONS]
             * Code-first expects there to be an Id 
             * property to create a primary key in the
             * database, but it's not necessary. We 
             * can use System.ComponentModel.DataAnnotations;
             *
             * Data annotations can be used to create 
             * primary keys out of strings.
             */
            [Key] // This is the data annotation
            public string Username { get; set; } // this is the key 
            public string DisplayName { get; set; }
        }



        // The context class represents a session with
        // the database. It can be used for querying 
        // and saving data.
        public class BlogContext : DbContext
        {
            // DbSet is used for every type in the 
            // model and allows for the creation of
            // instances of that model.
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<User> Users { get; set; }
        }
    }
}
