param(
[string] $cifFile,    # National Rail Open Data CIF file to
[string] $database = "Timetable" # Database to load into
)

# Create database, uncomment if neccessary.
# Invoke-Sqlcmd -Database -InputFile .\Database\Scripts\CreateTimetable.sql
# Invoke-Sqlcmd -Database -InputFile .\Database\Scripts\CreateTimetableRdg.sql

Write-Host "Create tables - $database";
Invoke-Sqlcmd -Database $database -InputFile "..\Database\Scripts\CreateSchema.sql";

Write-Host "Load $cifFile - $database"
Start-Process -FilePath 'dotnet' -Wait -ArgumentList ".\TimetableLoader\bin\Debug\netcoreapp2.2\TimetableLoader.dll -i $cifFile -d $database";

Write-Host "Add indices - $database";
Invoke-Sqlcmd -Database $database -InputFile ..\Database\Scripts\CreateIndices.sql;