using System;
using CameraService.Core.Entities;
using CameraWebService.DAL.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AForge.Video;
using CameraService.Core.CameraStreamService;
using CameraWebService.Models;

namespace CameraWebService.Controllers
{
    public class CamerasController : Controller
    {
        private readonly CameraServerDataModel _db = new CameraServerDataModel();
        private readonly ICameraStreamSaver _cameraStreamSaver;

        public CamerasController(ICameraStreamSaver cameraStreamSaver)
        {
            _cameraStreamSaver = cameraStreamSaver;
        }

        // GET: Cameras
        public ActionResult Index()
        {
            return View(_db.Cameras.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var camera = _db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IpAddress")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                _db.Cameras.Add(camera);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(camera);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var camera = _db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IpAddress")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(camera).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(camera);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var camera = _db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var camera = _db.Cameras.Find(id);
            _db.Cameras.Remove(camera ?? throw new InvalidOperationException());
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CameraViewer(Camera camera)
        {
            var stream = _cameraStreamSaver.GetCameraCaptureStream(camera.Id);
            var vm = new CameraViewModel(camera, stream);

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}