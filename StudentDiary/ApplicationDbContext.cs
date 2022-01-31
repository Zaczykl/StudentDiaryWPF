using StudentDiary.Models.Configurations;
using StudentDiary.Models.Domains;
using System;
using System.Data.Entity;
using System.Linq;

namespace StudentDiary
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext()
            : base(ConnectionStringBuild.sqlconnectionstringbuilder().ConnectionString)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
        }

    }
}