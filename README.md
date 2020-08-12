## Recommended tools
To follow up with these instruction use:
1. Visual Studio 2017
1. Microsoft SQL Server 2019
1. Microsoft SQL Server Managment Studio (SMSS)

For addition information you can email me at *esaric5@gmail.com*
## Build and Run Instructions
To run this solution follow these steps:

* Clone or download this repository

* In Microsoft SQL Server Managment Studio (SMSS) select your local SQL Server (SQLEXPRESS) and select `Connect`

   * Right click on `Databases` and select `Import Data-tier Application...`
   
   * At **Introduction** select `Next`
   
   * At **Import Settings** select `Import from local disk`, then select `Browse`, navigate to directory where you cloned/downloaded the repository and select **BloggingPlatform/Database/BloggingPlatform.bacpac**, then select `Next` 
   
   * At **Datebase Settings** enter optional database name (or leave default) and select `Next`
   
   * At **Summary** select `Finish` and wait for process to finish then select `Close`

* In Visual Studio 2017 go to `File` > `Open` > `Project/Solution` and select **BloggingPlatform.sln** from directory where you cloned/downloaded the repository

   * Open **application.json** and set database name you defined in SMSS (optionally set your SQL server). If your database name is **BPDB**, then in database connection string you should set: *"Server=.\\SQLEXPRESS;Database=__BPDB__;Trusted_Connection=True;"*
   * Run the solution with `F5` or go `Debug` > `Start debugging`
