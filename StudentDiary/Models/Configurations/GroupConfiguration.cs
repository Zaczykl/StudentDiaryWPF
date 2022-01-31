using StudentDiary.Models.Domains;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
namespace StudentDiary.Models.Configurations
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups");
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
