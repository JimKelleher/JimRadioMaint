using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//-------------------------------------------
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;

namespace JimRadioMaint.Models
{
    [Table("video_code", Schema = "dbo")]

    public class VideoCode
    {
        [Key]
        public int id { get; set; }        

        [DisplayName("Column")]
        public string video_column { get; set; }

        [DisplayName("Value")]
        public string code_value { get; set; }

        [DisplayName("Description")]
        public string code_description { get; set; }

        [DisplayName("Icon")]
        public string code_icon { get; set; }
    }
    
    public class VideoCodeDBContext : DbContext
    {
        public DbSet<VideoCode> VideoCodes { get; set; }

        public VideoCodeDBContext() : base("VideoDBContext") { }
    }
}
