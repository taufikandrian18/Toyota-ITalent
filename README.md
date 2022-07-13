# ITalent Web

Pre-requisite:

1. Install Node.js
2. Install npm
3. Install ipack - Open cmd/command prompt and run "**npm install -g instapack@7.6.1**"
4. Install SDK 2.2.207 (https://dotnet.microsoft.com/download/dotnet-core/2.2)

Steps:

1. Masuk ke dalam folder "..\ITalent_Web\Talent.WebAdmin" dari folder source code.
2. Buka cmd/command prompt pada folder "..\Talent.WebAdmin"
3. Run command "**ipack**" pada cmd.
4. Run command "**dotnet publish -c release**" pada cmd.
5. Folder Release akan terbuat di path ".."..\Talent.WebAdmin\bin\Release\netcoreapp2.2\publish".

Note: folder publish tersebut yang akan dicopy ke server production.


