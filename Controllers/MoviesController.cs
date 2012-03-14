// =====COPYRIGHT=====
// sssssssssssddddddddddddfffffffff
// =====COPYRIGHT=====
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        //
        // GET: /Movies/
        public List<MvcMovie.Models.Movie> GetAllMovies()
        {
            List<Movie> lstMoview = new List<Movie>();

            Database db = DatabaseFactory.CreateDatabase("cnnString");
            DbCommand dbCommand = db.GetSqlStringCommand("select * from Movies");
            IDataReader dr = db.ExecuteReader(dbCommand);
            while (dr.Read())
            {
                Movie mv = new Movie();
                mv.ID = Convert.ToInt32(dr["ID"]);
                mv.Title = dr["Title"].ToString();
                mv.ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"].ToString());
                mv.Genre = dr["Genre"].ToString();
                mv.Price = Convert.ToDecimal(dr["Price"].ToString());
                lstMoview.Add(mv);
            }
            return lstMoview;
        }
        public Movie GetMoviesByID(int id)
        {
            List<Movie> lstMoview = new List<Movie>();

            Database db = DatabaseFactory.CreateDatabase("cnnString");
            DbCommand dbCommand = db.GetSqlStringCommand("select * from Movies where ID=" + id);
            IDataReader dr = db.ExecuteReader(dbCommand);
            while (dr.Read())
            {
                Movie mv = new Movie();
                mv.ID = Convert.ToInt32(dr["ID"]);
                mv.Title = dr["Title"].ToString();
                mv.ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"].ToString());
                mv.Genre = dr["Genre"].ToString();
                mv.Price = Convert.ToDecimal(dr["Price"].ToString());
                lstMoview.Add(mv);
            }
            if (lstMoview.Count > 0)
                return lstMoview.First();
            else
            {
                return null;
            }
        }
        public ActionResult Index()
        {
            List<Movie> lstMoview = GetAllMovies();
            return View(lstMoview);
        }

        //
        // GET: /Movies/Details/5

        public ActionResult Details(int id)
        {

            Movie movie = GetMoviesByID(id);
            if (movie != null)
                return View(movie);

            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /Movies/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Movies/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Movie mv=new Movie();
                mv.Title = collection["Title"].ToString();
                mv.ReleaseDate = Convert.ToDateTime(collection["ReleaseDate"].ToString());
                mv.Genre = collection["Genre"].ToString();
                mv.Price = Convert.ToDecimal(collection["Price"].ToString());
                InsertMovie(mv);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private int InsertMovie(Movie mv)
        {
            int res = 0;
            Database db = DatabaseFactory.CreateDatabase("cnnString");
            DbCommand dbCommand = db.GetSqlStringCommand("Insert Movies values(@Title,@ReleaseDate,@Genre,@Price)");
            db.AddInParameter(dbCommand, "Title", DbType.String, mv.Title);
            db.AddInParameter(dbCommand, "ReleaseDate", DbType.Date, mv.ReleaseDate);
            db.AddInParameter(dbCommand, "Genre", DbType.String, mv.Genre);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, mv.Price);
            res = db.ExecuteNonQuery(dbCommand);
            return res; 
        }
        //
        // GET: /Movies/Edit/5

        public ActionResult Edit(int id)
        {
            Movie movie = GetMoviesByID(id);
            if (movie != null)
                return View(movie);
            else
                return HttpNotFound();
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                UpdateMovie(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        private int UpdateMovie(Movie mv)
        {
            int res = 0;
            Database db = DatabaseFactory.CreateDatabase("cnnString");
            DbCommand dbCommand = db.GetSqlStringCommand("Update Movies set Title=@Title, ReleaseDate=@ReleaseDate,Genre=@Genre,Price=@Price where ID=@ID");
            db.AddInParameter(dbCommand, "Title", DbType.String, mv.Title);
            db.AddInParameter(dbCommand, "ReleaseDate", DbType.Date, mv.ReleaseDate);
            db.AddInParameter(dbCommand, "Genre", DbType.String, mv.Genre);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, mv.Price);
            db.AddInParameter(dbCommand, "ID", DbType.Int32, mv.ID);
            res=db.ExecuteNonQuery(dbCommand);
            return res;
        }




        //
        // GET: /Movies/Delete/5
        public ActionResult Delete(int id)
        {
            List<MvcMovie.Models.Movie> lstMoview = new List<Movie>();
            for (int i = 0; i < 5; i++)
            {
                Movie mv = new Movie();
                mv.ID = i;
                mv.Title = "film " + i;
                mv.Genre = "film " + i;
                mv.Price = i;
                lstMoview.Add(mv);
            }
            IEnumerable<Movie> obj = from i in lstMoview where i.ID == id select i;
            if (obj.Count() > 0)
            {
                Movie movie = (from i in lstMoview where i.ID == id select i).First();
                return Content(Boolean.TrueString);
            }
            else
            {
                return Content(Boolean.FalseString);
            }
        }

        //
        // POST: /Movies/Delete/5

        [HttpPost]
        public ActionResult Delete(Movie movie)
        {
            try
            {
                // TODO: Add delete logic here
                DeleteMovie(movie.ID);
                return JavaScript("window.top.location.href ='Movies';");

            }
            catch
            {
                return View();
            }
        }

        private int DeleteMovie(int id)
        {
            int res = 0;
            Database db = DatabaseFactory.CreateDatabase("cnnString");
            DbCommand dbCommand = db.GetSqlStringCommand("Delete from Movies where ID=@ID");
            db.AddInParameter(dbCommand, "ID", DbType.Int32, id);
            res = db.ExecuteNonQuery(dbCommand);
            return res;
        }

        public ActionResult SearchIndex(string movieGenre, string searchString)
        {
            List<MvcMovie.Models.Movie> lstMoview = new List<Movie>();
            for (int i = 0; i < 5; i++)
            {
                Movie mv = new Movie();
                mv.ID = i;
                mv.Title = "film " + i;
                mv.Genre = "genre " + i;
                lstMoview.Add(mv);
            }
            var GenreLst = new List<string>();

            var GenreQry = from d in lstMoview
                           orderby d.Genre
                           select d.Genre;
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            IEnumerable<Movie> movies = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = lstMoview.Where(s => s.Title.Contains(searchString));
            }
            if (string.IsNullOrEmpty(movieGenre))
                return View(movies);
            else
            {
                return View(movies.Where(x => x.Genre == movieGenre));
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken(Salt = "PostData")]
        public ActionResult PostData(string sm1, string sm2)
        {
            var btn = sm1 ?? sm2;
            return RedirectToAction("Index");
        }
    }
}
