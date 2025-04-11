# Music_Band_Website

Before start:
1. In Program.cs
   options.UseSqlServer("Server=localhost\\[ put your server connection here ];Database=Music_BandDB;Trusted_Connection=True;TrustServerCertificate=True;");
   Sql Server Managment System -> server name

2. (Optional) For emails:
   Go to Mailer.cs and change
   From -> your_email address
   Generate app password and set it to "GMAIL_APP_PASSWORD"
   in cmd: (setx GMAIL_APP_PASSWORD "your_key")
