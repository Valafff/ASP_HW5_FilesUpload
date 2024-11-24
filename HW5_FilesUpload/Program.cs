var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
	var responce = context.Response;
	var request = context.Request;

	responce.ContentType = "text/html; charset=utf-8";

	if (request.Path == "/upload" && request.Method == "POST")
	{
		IFormFileCollection files = request.Form.Files;
		if (files.Count == 2)
		{
			var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
			Directory.CreateDirectory(uploadPath);
			foreach (var file in files)
			{
				string fullPath = $"{uploadPath}/{file.FileName}";
				using (var fs = new FileStream(fullPath, FileMode.Create))
				{
					await file.CopyToAsync(fs);
				}
			}
			await responce.WriteAsync("Файлы успешно загружены");
		}
		else
		{
			await responce.WriteAsync("Ошибка загрузки файлов");
		}
		
	}
	else
	{
		await responce.SendFileAsync("html/index.html");
	}

});
app.Run();
