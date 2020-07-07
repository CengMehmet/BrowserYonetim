using System;

namespace BrowserYonetim
{
    public class BrowserLog
    {
        public int id { get; set; }
        public int UserId { get; set; }
        virtual public User User { get; set; }
        public string Adres { get; set; }
        public DateTime Tarih { get; set; }
    }
}