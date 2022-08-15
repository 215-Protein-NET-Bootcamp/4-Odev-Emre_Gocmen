using FluentMigrator;
using System;

namespace PaginationCacheAPI
{
    [Migration(202208150930,"initial")]
    public class InitialMigration : Migration
    {
        public override void Down()
        {// db geri alir.
            #region Tables
            Delete.Table("Product");
            #endregion
        }

        public override void Up()
        {// db degisiklikler gerceklesir.
            #region Tables
            Create.Table("Person")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("CreatedBy").AsString(100).NotNullable()
                .WithColumn("CreatedAt").AsDate()
                .WithColumn("StaffId").AsString(100)
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("Phone").AsString().NotNullable()
                .WithColumn("DateOfBirth").AsDate().NotNullable();

            #endregion

            #region InitialData

            Insert.IntoTable("Person")
                .Row(new
                {
                    CreatedBy = "Emre",
                    CreatedAt = DateTime.Now,
                    StaffId = "Emre",
                    FirstName = "Emre",
                    LastName = "Emre",
                    Email = "Emre",
                    Description = "Emre",
                    Phone = "Emre3393",
                    DateOfBirth = DateTime.UtcNow
                });

            #endregion

        }
    }
}
