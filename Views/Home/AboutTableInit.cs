using System.Web.UI;

namespace About
{
    public class AboutTableInit : AboutTableInitBase
    {
        // NOTE: The constructor will pass a reference to the AboutTable object
        // (basically an HTML table) and all the work done in the base class will
        // act upon it.

        // Each application will define its own version of this class and, in this
        // way, I will accomplish a unique About window for each application.

      //public AboutTableInit(UserControl AboutTable                   ) : base(AboutTable)
        public AboutTableInit(System.Web.Mvc.ViewUserControl AboutTable) : base(AboutTable)
        {
            this.set_cell_group("row1_column1", "<br/><br/>Framework", "/JimRadioMaint/Images/About/dot_net_4_2.jpg", "Microsoft .NET", "4.0");
            this.set_cell_group("row1_column2", "JimRadio<br/>Database<br/>Maintenance", "/JimRadioMaint/Images/About/asp.jpg", "ASP", "2015");
            this.set_cell_group("row1_column3", "<br/><br/>Implementation", "/JimRadioMaint/Images/About/c_sharp_2010.jpg", "Visual C# &<br/>JavaScript", "2015");

            this.set_cell_group("row2_column1", "<br/><br/>Design Pattern", "/JimRadioMaint/Images/About/MVC3.jpg", "MVC & Razor", "3");
            this.set_cell_group("row2_column2", "<br/><br/>Storage", "/JimRadioMaint/Images/About/sql_server.JPG", "SQL Server", "2005 / 9");
            this.set_cell_group("row2_column3", "<br/><br/>Hosting", "/JimRadioMaint/Images/About/godaddy.jpg", "GoDaddy.com", "");

            this.set_cell_group("row3_column1", "<br/>Video Content<br/>& APIs", "/JimRadioMaint/Images/About/google_youtube_data_api_1_4.jpg", "Google/YouTube", "");
            this.set_cell_group("row3_column2", "<br/><br/>Video Player", "/JimRadioMaint/Images/About/flashplayer.JPG", "Adobe Flash", "");
            this.set_cell_group("row3_column3", "<br/><br/>Video Player", "/JimRadioMaint/Images/About/HTML_5_video.png", "HTML", "5");

            this.set_cell_group("row4_column1", "<br/><br/>Release", "/JimRadioMaint/Images/About/3.png", "", "");
            this.set_cell_group("row4_column2", "<br/><br/>Programming", "/JimRadioMaint/Images/About/programmer.jpg", "Jim Kelleher", "");
            this.set_cell_group("row4_column3", "", "", "", "");

        }

    }

}
