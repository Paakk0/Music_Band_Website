using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Music_Band_Website.Model
{
    //--------------------------------------------------------User--------------------------------------------------------
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone_Number { get; set; }
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
        public DateTime Registration_Date { get; set; }
        public bool Is_Admin { get; set; }

        public User(int id, string f_name, string l_name, string phone, string password, bool is_admin)
        {
            Id = id;
            First_Name = f_name;
            Last_Name = l_name;
            Phone_Number = phone;
            Email = password;
            Password = password;
            Is_Admin = is_admin;
        }
        public User()
        {
            
        }
    }

    //--------------------------------------------------------Subscription--------------------------------------------------------
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }

    //--------------------------------------------------------News--------------------------------------------------------
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [AllowNull]
        public string Details { get; set; }
        [AllowNull]
        public string Image { get; set; }
    }

    //--------------------------------------------------------Music_Type--------------------------------------------------------
    public class Music_Type
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
    }

    //--------------------------------------------------------Concert--------------------------------------------------------
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public DateTime DateTime { get; set; }
    }

    //--------------------------------------------------------Song--------------------------------------------------------
    public class Song
    {
        [Key]
        public int Id { get; set; }
        public int Id_Type { get; set; }
        public string Title { get; set; }
        public byte[] Audio { get; set; }
        public string Cover_Image { get; set; }
        public string Description { get; set; }


        [ForeignKey(nameof(Id_Type))]
        public Music_Type Music_Type { get; set; }
    }

    //--------------------------------------------------------Saved_Concert--------------------------------------------------------
    [Keyless]
    public class Liked_Song
    {
        public int Id_User { get; set; }
        public int Id_Song { get; set; }

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; }

        [ForeignKey(nameof(Id_Song))]
        public Song Song { get; set; }
    }

    //--------------------------------------------------------Saved_Concert--------------------------------------------------------
    [Keyless]
    public class Event_Playlist
    {
        public int Id_Event { get; set; }
        public int Id_Song { get; set; }

        [ForeignKey(nameof(Id_Song))]
        [MaybeNull]
        public Song Song { get; set; }

        [ForeignKey(nameof(Id_Event))]
        [MaybeNull]
        public Event Event { get; set; }
    }

    [Keyless]
    public class Event_Tickets
    {
        public int Id_Event { get; set; }
        public int Id_User { get; set; }
        public int TicketAmount { get; set; }

        [ForeignKey(nameof(Id_Event))]
        public Event Event { get; set; }

        [ForeignKey(nameof(Id_User))]
        public User User { get; set; }
    }
}