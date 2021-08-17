using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("agents")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("agentUrl").AsString();
            Create.Table("allcpumetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
            Create.Table("alldotnetmetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
            Create.Table("allhddmetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
            Create.Table("allnetworkmetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
            Create.Table("allrammetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64().ForeignKey()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();            
        }

        public override void Down()
        {
            Delete.Table("allcpumetrics");
            Delete.Table("alldotnetmetrics");
            Delete.Table("allhddmetrics");
            Delete.Table("allnetworkmetrics");
            Delete.Table("allrammetrics");
        }
    }

}
