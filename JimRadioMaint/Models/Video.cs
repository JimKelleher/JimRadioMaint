using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//-----------------------
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace JimRadioMaint.Models
{
    [Table("video_mvc", Schema = "dbo")]

    public class Video
    {
        [Key]
        public int id { get; set; }
        
        [Display(Name = "Title")]
        public string youtube_title { get; set; }

        [DisplayName("ID")]
        public string youtube_id { get; set; }

        [Display(Name = "Duration")]
        public int duration_seconds { get; set; }

        [DisplayName("Genre")]
        public string genre { get; set; }

        [DisplayName("Rating")]
        public int rating { get; set; }

        [DisplayName("Played")]
        public string played { get; set; }

        [DisplayName("Play Error")]
        public System.Nullable<int> play_error { get; set; }
        
        public class VideoDBContext : DbContext
        {
            public DbSet<Video> Videos { get; set; }

            public VideoDBContext() : base("VideoDBContext") { }
        }
    }
}