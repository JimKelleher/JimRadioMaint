using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//-------------------------
using JimRadioMaint.Models;

namespace JimRadioMaint.Controllers
{ 
    public class VideoController : Controller
    {
        private Video.VideoDBContext videoDBContext =     new Video.VideoDBContext();
        private VideoCodeDBContext   videoCodeDBContext = new VideoCodeDBContext();

        //
        // GET: /Video/

        //public ViewResult Index()
        //{
        //    // Produce the view:
        //    //return View(videoModel);
        //}

        public ViewResult Index(ICollection<string> titles, string genre = "", int rating = 0)
        {
            // Initialize the retrieval arguments, which will be passed by way of the ViewBag:
            ViewBag.genre = "";
            ViewBag.rating = 0;
            ViewBag.searchCriteria = "";

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // NOTE: The query below, is the logical goal of this SQL construction processing.  It
            // is accomplished in two stages, first handling the single-value criteria of Genre and
            // Rating.  Secondly, handling the multi-value criterion of Title.
            //
            // The "extended" criteria, below, have their youtube_id's collected into an arry, thus
            // allowing the query to be INCLUSIVE, and positive, and uncluttered by OR's.
            //
            // The array is compared to the column values in, kind of, a backwards SQL IN clause:
            //
            // "where youTubeIds.Contains(result.youtube_id)"
            //
            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // Logical goal of SQL construction:
            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            //   select youtube_title,
            //          youtube_id,
            //          duration_seconds,
            //          genre,
            //          rating,
            //          played,
            //          play_error
            //     from video
            //    where genre = 'ROCK'                      // BASE
            //      and rating = 3                          // BASE
            //      and (youtube_title like '%bent%' or     // EXTENDED
            //           youtube_title like '%zero 7%'      // EXTENDED
            //          )
            // order by youtube_title

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Load reference data into the ViewBag:
            LoadReferenceData();

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // This will contain the Extended key values:
            List<string> youTubeIds = new List<string>();
            
            // NOTE: All of the following var's are of type IEnumerable<Video>

            // For development:
            //var videoModelAll = from result in videoDBContext.Videos.Take(5)
            var videoModelAll =    from result in videoDBContext.Videos
                                orderby result.youtube_title
                                  where 1 == 1      // NOTE: Without this, the Where processing,
                                 select result;     // below, would not be available.

            var videoModelContains =    from result in videoDBContext.Videos
                                     orderby result.youtube_title
                                       where youTubeIds.Contains(result.youtube_id)
                                      select result;

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Base / AND:
            var baseVideoModelViaAnd = videoModelAll;

            if (genre != "")
            {
                // Modify the result set to find the retrieval argument:
                baseVideoModelViaAnd = baseVideoModelViaAnd.Where(result => result.genre == genre);
                
                // Pass the retrieval argument by way of the ViewBag:
                ViewBag.genre = genre;
            }
            
            if (rating != 0)
            {
                // Modify the result set to find the retrieval argument:
                baseVideoModelViaAnd = baseVideoModelViaAnd.Where(result => result.rating == rating);

                // Pass the retrieval argument by way of the ViewBag:
                ViewBag.rating = rating;
            }

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Extended / OR:
            var extendedVideoModelViaOr = Enumerable.Empty<Video>();

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            if (titles == null)
            {
                // Handles (1) no parameters, (2) base parameters only:
                return View(baseVideoModelViaAnd);
            }
            else
            {
                // Init:
                int index = 0;

                foreach (string title in titles)
                {
                    // Modify the result set to find the retrieval argument(s) and then
                    // pass the retrieval argument(s) by way of the ViewBag:

                    // Base / AND:
                    extendedVideoModelViaOr =
                        baseVideoModelViaAnd.Where(result => result.youtube_title.Contains(title));

                    // Extended / OR:
                    foreach (var videoItem in extendedVideoModelViaOr)
                    {
                        youTubeIds.Add(videoItem.youtube_id);
                    }

                    // If appropriate, concatenate the comma:
                    if (index != 0) { ViewBag.searchCriteria += ", "; }

                    // Concatenate the title:
                    ViewBag.searchCriteria += title;

                    // Prep for the next time through:
                    index++;
               }

                // NOTE: At this point, list youTubeIds has been filled.  Thus, the query videoModelContains
                // has all it needs to execute.

                // Handles (3) base + extended parameters, (4) extended parameters only:
                return View(videoModelContains);
            }
        }

        public void LoadReferenceData()
        {
            //--------------------------------------------------------------------------------------
            // GENRES:
            // Load up the reference data for genres.  NOTE: This structure is referenced below
            ViewBag.Genres =    from result in videoCodeDBContext.VideoCodes
                             orderby result.code_description
                               where result.video_column == "genre"
                              select result;

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // Load up the Dictionaries for genres:
            ViewBag.dictionaryGenre = new Dictionary<string, string> { };
            ViewBag.dictionaryGenreIcon = new Dictionary<string, string> { };

            foreach (var genreItem in ViewBag.Genres)
            {
                ViewBag.dictionaryGenre.Add(genreItem.code_value, genreItem.code_description);
                ViewBag.dictionaryGenreIcon.Add(genreItem.code_value, genreItem.code_icon);
            }

            //--------------------------------------------------------------------------------------
            // RATINGS:
            // Load up the reference data for ratings.  NOTE: This structure is referenced below
            ViewBag.Ratings =    from result in videoCodeDBContext.VideoCodes
                              orderby result.code_value
                                where result.video_column == "rating"
                               select result;

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // Load up the Dictionaries for ratings:
            ViewBag.dictionaryRating = new Dictionary<string, string> { };
            ViewBag.dictionaryRatingIcon = new Dictionary<string, string> { };

            foreach (var ratingItem in ViewBag.Ratings)
            {
                ViewBag.dictionaryRating.Add(ratingItem.code_value, ratingItem.code_description);
                ViewBag.dictionaryRatingIcon.Add(ratingItem.code_value, ratingItem.code_icon);
            }

            //--------------------------------------------------------------------------------------
            // PLAY ERRORS:
            // Load up the reference data for play errors.  NOTE: This structure is referenced below
            ViewBag.PlayErrors =    from result in videoCodeDBContext.VideoCodes
                                 orderby result.code_description
                                   where result.video_column == "play_error"
                                  select result;

            //- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
            // Load up the Dictionaries for play errors:
            ViewBag.dictionaryPlayError = new Dictionary<string, string> { };
            ViewBag.dictionaryPlayErrorIcon = new Dictionary<string, string> { };

            foreach (var playErrorItem in ViewBag.PlayErrors)
            {
                ViewBag.dictionaryPlayError.Add(playErrorItem.code_value, playErrorItem.code_description);
                ViewBag.dictionaryPlayErrorIcon.Add(playErrorItem.code_value, playErrorItem.code_icon);
            }

            //--------------------------------------------------------------------------------------
        }        

        //
        // GET: /Video/Edit/5
 
        public ActionResult Edit(int id)
        {
            // Load reference data into the ViewBag:
            LoadReferenceData();

            Video video = videoDBContext.Videos.Find(id);
            return View(video);
        }

        //
        // POST: /Video/Edit/5

        [HttpPost]
        public ActionResult Edit(Video video)
        {
            if (ModelState.IsValid)
            {
                videoDBContext.Entry(video).State = EntityState.Modified;
                videoDBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        //
        // GET: /Video/Delete/5
 
        public ActionResult Delete(int id)
        {
            Video video = videoDBContext.Videos.Find(id);
            return View(video);
        }

        //
        // POST: /Video/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Video video = videoDBContext.Videos.Find(id);
            videoDBContext.Videos.Remove(video);
            videoDBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            videoDBContext.Dispose();
            base.Dispose(disposing);
        }
    }
}