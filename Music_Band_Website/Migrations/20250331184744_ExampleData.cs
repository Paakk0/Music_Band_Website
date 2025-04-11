using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Music_Band_Website.Migrations
{
    /// <inheritdoc />
    public partial class ExampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "First_Name", "Last_Name", "Phone_Number", "Email", "Password", "Registration_Date", "Is_Admin" },
                values: new object[,]
                {
                    //parola 1-8
                     { 1, "Nikolai",   "Iliev", "0987654321", "example1@gmail.com", "73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=", "2025-03-01", true },
                     { 2, "User",   "Userov", "0987654321", "example2@gmail.com", "73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=", "2025-03-02", false }
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "Title", "Description", "Details", "Image" },
                values: new object[,]
                {
                    { 1, "Ново издание на албума", "Групата Mental пусна нова версия на дебютния си албум.", "Новото издание включва два бонус трака и ремастериран звук.", "/images/news1.jpg" }
                });
            InsertMusicTypes(migrationBuilder);
            InsertSongs(migrationBuilder);
            InsertEvents(migrationBuilder);
        }

        private void InsertMusicTypes(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Music_Type",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    {1, "Groove"}
                });
        }

        private void InsertSongs(MigrationBuilder migrationBuilder)
        {
            string filePath = Path.Combine("wwwroot", "uploads", "song1.mp3");
            byte[] audioBytes = File.ReadAllBytes(filePath);

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "Id_Type", "Title", "Audio", "Cover_Image", "Description" },
                values: new object[,]
                {
                    { 1, 1, "Song1", audioBytes, "/images/group_photo.jpg", "This is our first song" }
                });
        }

        private void InsertEvents(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "City", "Location", "DateTime" },
                values: new object[,]
                {
                    {1,"Varna","Morskata",new DateTime(2025,03,20,20,0,0)},
                    {2,"Sofia","Center",new DateTime(2025,04,24,21,0,0)}
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
