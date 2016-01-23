namespace RAMS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        UserType = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        UserStatus = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        JobTitle = c.String(nullable: false, maxLength: 100),
                        Company = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 300),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AdminId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        AgentId = c.Int(),
                        ClientId = c.Int(),
                        AdminId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 100),
                        Details = c.String(nullable: false, maxLength: 300),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.Admins", t => t.AdminId, cascadeDelete: true)
                .ForeignKey("dbo.Agents", t => t.AgentId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.AgentId)
                .Index(t => t.ClientId)
                .Index(t => t.AdminId);
            
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        AgentId = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        AgentStatus = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 100),
                        UserType = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        UserStatus = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        JobTitle = c.String(nullable: false, maxLength: 100),
                        Company = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 300),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.AgentId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Interviews",
                c => new
                    {
                        InterviewId = c.Int(nullable: false, identity: true),
                        CandidateId = c.Int(nullable: false),
                        InterviewerId = c.Int(nullable: false),
                        InterviewDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InterviewId)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .ForeignKey("dbo.Agents", t => t.InterviewerId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.InterviewerId);
            
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        CandidateId = c.Int(nullable: false, identity: true),
                        PositionId = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 300),
                        Country = c.String(nullable: false, maxLength: 100),
                        PostalCode = c.String(nullable: false, maxLength: 10),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Feedback = c.String(maxLength: 1000),
                        FileName = c.String(),
                        MediaType = c.String(),
                        FileContent = c.Binary(),
                        Score = c.Int(),
                        Status = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CandidateId)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        CleintId = c.Int(nullable: false),
                        AgentId = c.Int(),
                        CategoryId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 2000),
                        CompanyDetails = c.String(nullable: false, maxLength: 1000),
                        Location = c.String(nullable: false, maxLength: 200),
                        Qualifications = c.String(nullable: false),
                        AssetSkills = c.String(),
                        PeopleNeeded = c.Int(nullable: false),
                        AcceptanceScore = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.PositionId)
                .ForeignKey("dbo.Agents", t => t.AgentId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Clients", t => t.CleintId, cascadeDelete: true)
                .Index(t => t.CleintId)
                .Index(t => t.AgentId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        UserType = c.Int(nullable: false),
                        Role = c.Int(nullable: false),
                        UserStatus = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 200),
                        LastName = c.String(nullable: false, maxLength: 200),
                        JobTitle = c.String(nullable: false, maxLength: 100),
                        Company = c.String(nullable: false, maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 300),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ClientId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Notifications", "AgentId", "dbo.Agents");
            DropForeignKey("dbo.Interviews", "InterviewerId", "dbo.Agents");
            DropForeignKey("dbo.Candidates", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Positions", "CleintId", "dbo.Clients");
            DropForeignKey("dbo.Positions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Positions", "AgentId", "dbo.Agents");
            DropForeignKey("dbo.Interviews", "CandidateId", "dbo.Candidates");
            DropForeignKey("dbo.Agents", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Notifications", "AdminId", "dbo.Admins");
            DropIndex("dbo.Clients", new[] { "Email" });
            DropIndex("dbo.Clients", new[] { "UserName" });
            DropIndex("dbo.Positions", new[] { "CategoryId" });
            DropIndex("dbo.Positions", new[] { "AgentId" });
            DropIndex("dbo.Positions", new[] { "CleintId" });
            DropIndex("dbo.Candidates", new[] { "PositionId" });
            DropIndex("dbo.Interviews", new[] { "InterviewerId" });
            DropIndex("dbo.Interviews", new[] { "CandidateId" });
            DropIndex("dbo.Agents", new[] { "Email" });
            DropIndex("dbo.Agents", new[] { "UserName" });
            DropIndex("dbo.Agents", new[] { "DepartmentId" });
            DropIndex("dbo.Notifications", new[] { "AdminId" });
            DropIndex("dbo.Notifications", new[] { "ClientId" });
            DropIndex("dbo.Notifications", new[] { "AgentId" });
            DropIndex("dbo.Admins", new[] { "Email" });
            DropIndex("dbo.Admins", new[] { "UserName" });
            DropTable("dbo.Clients");
            DropTable("dbo.Categories");
            DropTable("dbo.Positions");
            DropTable("dbo.Candidates");
            DropTable("dbo.Interviews");
            DropTable("dbo.Departments");
            DropTable("dbo.Agents");
            DropTable("dbo.Notifications");
            DropTable("dbo.Admins");
        }
    }
}
