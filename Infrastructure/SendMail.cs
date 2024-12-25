using SendGrid;
using SendGrid.Helpers.Mail;

public class SendMail
{
    private readonly string _apiKey;

    public SendMail(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"];  // API key'i appsettings.json dosyasina koymak lazim, ancak github izin vermedi
    }
    public async Task Execute(ContactVM model)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress("kasimpasaoglu@windowslive.com", "Wissen Store");
        var to = new EmailAddress("kasim.pasaoglu@gmail.com");
        var subject = model.Subject;
        var plainTextContent = model.Message;
        var htmlContent = $"<h2>{model.Subject}</h2><p>{model.Message}</p><h4>{model.Email}</h4>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            throw new Exception($"Error. Status Code: {response.StatusCode}, Body: {responseBody}");
        }
    }
}