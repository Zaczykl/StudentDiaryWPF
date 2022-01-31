namespace StudentDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedStudentTableNameAndGroupTableNameAddedRequirementsToStudentTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Group", newName: "Groups");
            RenameTable(name: "dbo.Rating", newName: "Ratings");
            AlterColumn("dbo.Students", "FirstName", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "FirstName", c => c.String());
            RenameTable(name: "dbo.Ratings", newName: "Rating");
            RenameTable(name: "dbo.Groups", newName: "Group");
        }
    }
}
