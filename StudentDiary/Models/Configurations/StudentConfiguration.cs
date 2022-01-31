using StudentDiary.Models.Domains;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace StudentDiary.Models.Configurations
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            ToTable("dbo.Students");
            HasKey(t => t.Id);
            Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
        }
    }
}
