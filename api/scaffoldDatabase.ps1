dotnet ef dbcontext scaffold "Name=ConnectionStrings:ChoreBoard" `
	Microsoft.EntityFrameworkCore.SqlServer `
	--force `
	--startup-project "ChoreBoard.Api" `
	--project "ChoreBoard.Data" `
	--output-dir "Models" `
	--context "ChoreBoardContext" `
	--use-database-names
