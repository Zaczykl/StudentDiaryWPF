using StudentDiary.Models.Domains;
using System.Data.Entity.ModelConfiguration;
namespace StudentDiary.Models.Configurations
{
    public class RatingConfiguration : EntityTypeConfiguration<Rating>
    {
        public RatingConfiguration()
        {
            ToTable("dbo.Ratings");
            HasKey(t => t.Id);

        }
    }
}
