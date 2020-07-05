using FluentMigrator;
using FluentMigrator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjeFinalOdevi.Migrations
{
    [Migration(1)]
    public class _001_Users_and_Roles : Migration
    {
        public override void Down()
        {
            Execute.Sql("DELETE FROM projeFinalOdevi.roles where name = 'user'");
            Execute.Sql("DELETE FROM projeFinalOdevi.roles where name = 'admin'");
            Delete.Table("role_users");
            Delete.Table("roles");
            Delete.Table("sales");
            Delete.Table("stock");
            Delete.Table("users");
        }

        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("userName").AsString(50).NotNullable()
                .WithColumn("eMail").AsString(50).NotNullable()
                .WithColumn("balance").AsInt32().NotNullable()
                .WithColumn("adSoyad").AsString(128).NotNullable()
                .WithColumn("addressMah").AsString(40).NotNullable()
                .WithColumn("addressCadSk").AsString(128).NotNullable()
                .WithColumn("addressil").AsString(128).NotNullable()
                .WithColumn("addressilce").AsString(128).NotNullable()
                .WithColumn("password_hash").AsString(256).NotNullable();
            
            Create.Table("stock")
                .WithColumn("stock_id").AsInt32().Identity().PrimaryKey()
                .WithColumn("description").AsString(256).NotNullable()
                .WithColumn("unitPrice").AsInt32().NotNullable()
                .WithColumn("belongsToUser").AsInt32().ForeignKey("users","id").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("quantityInStock").AsInt32().NotNullable();

            Create.Table("sales")
                .WithColumn("sales_id").AsInt32().Identity().PrimaryKey()
                .WithColumn("stock_id").AsInt32().ForeignKey("stock", "stock_id").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("belongsToUser").AsInt32().ForeignKey("users", "id").OnDelete(System.Data.Rule.Cascade).NotNullable()
                .WithColumn("date").AsDateTime().NotNullable()
                .WithColumn("quantity").AsInt32().NotNullable()
                .WithColumn("sale_sum").AsInt32().NotNullable();

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            Create.Table("role_users")
                .WithColumn("userid").AsInt32().ForeignKey("users", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("roleid").AsInt32().ForeignKey("roles", "id").OnDelete(System.Data.Rule.Cascade);

            Execute.Sql("INSERT INTO projeFinalOdevi.roles (name) VALUES ('admin')");
            Execute.Sql("INSERT INTO projeFinalOdevi.roles (name) VALUES ('user')");

            /* CMD line;
             migrate -a D:\ProjeFinalOdevi\ProjeFinalOdevi\bin\ProjeFinalOdevi.dll -db MySql -conn "Data Source=127.0.0.1;Database=projefinalodevi;uid=root;pwd=MySQLPassis8420;"
            *//* CMD line;
             migrate -a C:\Users\Atabay\source\repos\ProjeFinalOdevi\ProjeFinalOdevi\bin\ProjeFinalOdevi.dll -db MySql -conn "Data Source=127.0.0.1;Database=ProjeFinalOdevinew;uid=root;pwd=root;"
            */
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void GetDownExpressions(IMigrationContext context)
        {
            base.GetDownExpressions(context);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void GetUpExpressions(IMigrationContext context)
        {
            base.GetUpExpressions(context);
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}