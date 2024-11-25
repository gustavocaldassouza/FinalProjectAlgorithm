using Algorithm;
using ConsoleApplicationSql;
using System.Data.SQLite;
string dbPath = "myDatabase.sqlite";

FinalProject finalProject = new FinalProject();
finalProject.HandleDatabase(dbPath);
finalProject.Exec();