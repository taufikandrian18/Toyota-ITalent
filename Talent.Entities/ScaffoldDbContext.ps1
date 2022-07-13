dotnet clean
Remove-Item *.cs

##############################################################
## Set these variables to match the project values
##############################################################

$contextName = "TalentContext";
$connectionString = "Data Source=localhost;Initial Catalog=Talent;Integrated Security=True";

##############################################################

Write-Host "Scaffolding database context... [scaffold-dbcontext.log]" -f Gray
dotnet ef dbcontext scaffold $connectionString "Microsoft.EntityFrameworkCore.SqlServer" -o "Entities" -d -f -c $contextName -v > scaffold-dbcontext.log
Write-Host "Scaffold completed! Don't forget to remove OnConfiguring method and add the DI constructor." -f Cyan 

$replace = "public $($contextName)(DbContextOptions<$($contextName)> options) : base(options) { }";
Write-Output $replace

$replace | clip;
Write-Host "Copied above snippet to clipboard." -f Gray
