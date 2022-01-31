namespace StudentDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Comments = c.String(),
                        Activities = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Group", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rate = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rating", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "GroupId", "dbo.Group");
            DropIndex("dbo.Rating", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "GroupId" });
            DropTable("dbo.Rating");
            DropTable("dbo.Students");
            DropTable("dbo.Group");
        }
    }
}
