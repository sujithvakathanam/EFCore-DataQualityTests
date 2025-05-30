
## Introduction
This project is to illustrate how EFcore can be used to create Data Quality tests within a Data Engineering project in .net world.<br>

EFCore(nuget package) is a lightweight, extensible ORM that helps in connecting to different databases using an object oriented language like C#<br>

Assuming that Extraction, Transformation and Loading is done by an ETL tool and the target tables in 'AdventureWorks' database is updated,
Data quality checks can be run on the target tables

## Workflow
<li>With EF CORE Power Tools- Dynamic POCO class creation can be done<br></li>
<li>Connects to datasource<br></li>
<li>Scan database table with queries<br></li>
<li>Review Results and Investigate Issues<br></li>

## SQL Server management studio
Following url contains instructions on how to install SSMS in your local machine
https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms

### AdventureWorks database
Following url contains instructions on how to download the 'Adventureworks' database .bak file
and restore it. You can use the .bak file to restore this sample database to your SQL Server instance
https://learn.microsoft.com/en-us/sql/samples/adventureworks-install-configure?view=sql-server-ver16&tabs=ssms

Once restored, your database can be accessed through SSMS as below
<img alt="img.png" src="img.png"/>

## Extensions
Efcore power tools extensions for visual studio. This will help us in creating POCO classes(Entities) by scanning the database<br>
https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools

For futher information on how to use EF Core power tools, please refer below link especially on how to 'Reverse Engineer a Database' <br>
https://code-maze.com/efcore-database-first-development-with-ef-core-power-tools/

## How to run the tests
Amend the appsettings.json file to point to your local sql servername<br>
Build the C# solution avaiable in EfCore-DataQuality with your favourite IDE <br>
Run the tests using Resharper or through Test Explorer <br>

## Docker
Existing data quality tests can be run with in a docker container provided the docker container is able to access another docker container hosting sql server in linux.
This is just for illustration purpose only. 
