using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAnnotationsConsoleApp
{
    /*
     * Data annotations are necessary when the domain classes
     * do not follow the conventions of EF
     * 
     * Note on DatabaseGenerated---
     * This is confusing and I don't really ge the purpose of
     * it. Check back later.
     * [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
     * public DateTime DateCreated { get; set; }
     */
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
    public class Passport
    {
        /*
         * TABLE---
         * The table in the database doesn't always match the
         * name of the class in the domain. This is how to specify
         * which table should be used or how the table should be 
         * defined in a code-first example.
         */
        [Table("InternalBlogs")]
        public class Blog
        {
            /*
             * KEYS---
             * The [Key] data annotation tells the database the
             * 'PrimaryTrackingKey' is the primary key for the
             * blog table when there is no 'clasnameId' style
             * naming.
             */
            [Key]
            public int PrimaryTrackingKey { get; set; }
            public string Title { get; set; }
            /*
             * MIN/MAX AND ERROR MESSAGES---
             * How to set the Min and Max length of a string value
             * and attach an error message. Many annotations allow
             * for error messages to be specified.
             * 
             * CONCURRENCY CHECK---
             * The original value will be used when SaveChanges is 
             * called. If the value changes, or if the original
             * value isn't given, an exception of 
             * 'DbUpdateConcurrencyException' will need to be handled.
             */
            [ConcurrencyCheck, MaxLength(10, ErrorMessage = "BloggerName must be 10 characters or less"), MinLength(5)]
            public string BloggerName { get; set; }
            /*
             * TIMESTAMP---
             * [TimeStamp] works like  concurrency check, but they
             * are non-nullable by default and there can be only one
             * of them per class.
             */
            [Timestamp]
            public Byte[] TimeStamp { get; set; }
            public virtual ICollection<Post> Posts { get; set; }
            /*
             * NOT MAPPED---
             * [NotMapped] tells ef not to map the property if it 
             * isn't meant to be stored in the database. This can
             * account for dynamic data like the example below.
             */
            [NotMapped]
            public string BlogCode
            {
                get
                {
                    return Title.Substring(0, 1) + ":" + BloggerName.Substring(0, 1);
                }
            }
            public BlogDetails BlogDetail { get; set; }
        }
        /*
         * COMPLEX TYPES---
         * It's not uncommon to have a model defined across classes.
         * This convention requires what is called a [ComplexType],
         * which can be tracked on their own, but only referenced
         * by the navigation property in the pseudo-parent class.
         */
        [ComplexType]
        public class BlogDetails
        {
            public DateTime? DateCreated { get; set; }
            /*
             * COLUMN---
             * Columns can be specific about the attributes it has.
             * The name, data type, and order can be defined using
             * [Column("name", prop = "propname")].
             */
            [Column("BlogDescription", TypeName = "ntext")]
            [MaxLength(250)]
            public string Description { get; set; }
        }
        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime DateCreated { get; set; }
            public string Content { get; set; }
            /*
             * INDEXES PART 1---
             * [Index] will name the index IX_PropertyName by default,
             * but you can change the name as it's done below.
             * 
             * Multi-column indexes are specified by using the same
             * name in multiple indexes and then ordering them.
             */
            // Index naming example: [Index("PostRatingIndex")]
            [Index("IX_BlogIdAndRating", 2)]// multi-column index for BlogId and Rating
            public int Rating { get; set; }
            [Index("IX_BlogIdAndRating", 1)]// multi-column index for BlogId and Rating
            public int BlogId { get; set; }
            /*
             * FOREIGN KEY---
             * Code first convention looks in the foreign class for
             * an Id suffix automatically, but since there is no BlogId
             * we specify by using [ForeignKey("BlogId")]. This will 
             * create a relationship between Post and Blog even though
             * Blog has no property of BlogId.
             */
            [ForeignKey("BlogId")]
            public Blog Blog { get; set; }
            public ICollection<Comment> Comments { get; set; }
            /*
             * Setup for multiple relationships with Person class
             * see 'INVERSE PROPERTY---' below.
             */
            public Person CreatedBy { get; set; }
            public Person UpdatedBy { get; set; }
        }
        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            /*
             * INVERSE PROPERTY---
             * Multiple relationships between classes are possible through
             * [InverseProperty] annotations. 
             */
            [InverseProperty("CreatedBy")]
            public List<Post> PostsWritten { get; set; }
            [InverseProperty("UpdatedBy")]
            public List<Post> PostsUpdated { get; set; }
        }
        public class Comment
        {
            //some comment props
        }
        /*
         * COMPOSITE KEYS---
         * This is how a composite key is set up with data
         * annoptations.
         * 
         * Dependancies:
         * using System.ComponentModel.DataAnnotations;
         * using System.ComponentModel.DataAnnotations.Schema;
         */
        [Key]
        [Column(Order=1)]
        public int PassportNumber { get; set; }
        [Key]
        [Column(Order = 2)]
        public string IssuingCounter { get; set; }
        /*
         * REQUIRED
         */
        [Required]
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
        public class User
        {
            public int UserId { get; set; }
            /*
             * INDEXES PART 2---
             * This will tell the database to index the column
             * and make it unique
             */
            [Index(IsUnique = true)]
            [StringLength(200)]
            public string Username { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
