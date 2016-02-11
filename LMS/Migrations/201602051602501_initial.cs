namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fils",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        schemaOccasionId = c.Int(nullable: false),
                        isShared = c.Boolean(nullable: false),
                        filePath = c.String(maxLength: 255),
                        owner = c.String(),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.schemaOccasions", t => t.schemaOccasionId, cascadeDelete: true)
                .Index(t => t.schemaOccasionId);
            
            CreateTable(
                "dbo.schemaOccasions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KlassId = c.Int(nullable: false),
                        name_for_schemaoccasion = c.String(),
                        description = c.String(),
                        path_to_inlamningsuppgift = c.String(),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Klasses", t => t.KlassId, cascadeDelete: true)
                .Index(t => t.KlassId);
            
            CreateTable(
                "dbo.Klasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.String(maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 30),
                        sharedFolder = c.String(maxLength: 255),
                        submitFolder = c.String(maxLength: 255),
                        startDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.TeacherId)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Name = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserKlasses",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Klass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Klass_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Klasses", t => t.Klass_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Klass_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Fils", "schemaOccasionId", "dbo.schemaOccasions");
            DropForeignKey("dbo.schemaOccasions", "KlassId", "dbo.Klasses");
            DropForeignKey("dbo.Klasses", "TeacherId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserKlasses", "Klass_Id", "dbo.Klasses");
            DropForeignKey("dbo.UserKlasses", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserKlasses", new[] { "Klass_Id" });
            DropIndex("dbo.UserKlasses", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Klasses", new[] { "TeacherId" });
            DropIndex("dbo.schemaOccasions", new[] { "KlassId" });
            DropIndex("dbo.Fils", new[] { "schemaOccasionId" });
            DropTable("dbo.UserKlasses");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Klasses");
            DropTable("dbo.schemaOccasions");
            DropTable("dbo.Fils");
        }
    }
}
