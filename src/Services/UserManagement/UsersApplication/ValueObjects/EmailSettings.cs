namespace UsersApplication.ValueObjects
{
    public interface IEmailSettings
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        bool UseSsl { get; }
        string SmtpName { get; }
        string SmtpPassword { get; }
        string SenderName { get; }
        string SenderEmail { get; }
    }

    public class EmailSettings : IEmailSettings
    {
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; } = 465;
        public bool UseSsl { get; set; } = true;
        public string SmtpName { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;        
        
    }
}
