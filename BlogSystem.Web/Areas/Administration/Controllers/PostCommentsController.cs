﻿using System;
using BlogSystem.Common;
using BlogSystem.Web.Areas.Administration.ViewModels.PostComments;
using BlogSystem.Web.Infrastructure.Mapping;

namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BlogSystem.Data.Models;
    using BlogSystem.Data.UnitOfWork;

    using PagedList;

    public class PostCommentsController : AdministrationController
    {
        public PostCommentsController(IBlogSystemData data) 
            : base(data)
        {
        }

        // GET: Administration/PostComments
        public ActionResult Index(int page = 1, int perPage = GlobalConstants.CommentsPerPageDefaultValue)
        {
            /* int pageNumber = page ?? 1;

             List<PostComment> postComments = this.Data.PostComments
                 .All()
                 .OrderByDescending(p => p.CreatedOn)
                 .Include(p => p.BlogPost)
                 .Include(p => p.User)
                 .ToList();*/

            int pagesCount = (int) Math.Ceiling(this.Data.Posts.All().Count() / (decimal)perPage);

            var comments = this.Data.PostComments
                .All()
                .Where(c => !c.IsDeleted)
                .OrderByDescending(p => p.CreatedOn)
                .Include(c => c.BlogPost)
                .Include(c => c.User)
                .To<PostCommentViewModel>();

            //PagedList<PostComment> model = new PagedList<PostComment>(postComments, pageNumber, GlobalConstants.CommentsPerPageDefaultValue);

            var model = new IndexPageViewModel
            {
                Comments = comments.ToList(),
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(model);
        }

        // GET: Administration/PostComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostComment postComment = this.Data.PostComments.Find(id);

            if (postComment == null)
            {
                return this.HttpNotFound();
            }

            return this.View(postComment);
        }

        // GET: Administration/PostComments/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostComment postComment = this.Data.PostComments.Find(id);

            if (postComment == null)
            {
                return this.HttpNotFound();
            }

            return this.View(postComment);
        }

        // POST: Administration/PostComments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostComment postComment)
        {
            if (this.ModelState.IsValid)
            {
                this.Data.PostComments.Update(postComment);
                this.Data.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(postComment);
        }

        // GET: Administration/PostComments/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostComment postComment = this.Data.PostComments.Find(id);

            if (postComment == null)
            {
                return this.HttpNotFound();
            }

            return this.View(postComment);
        }

        // POST: Administration/PostComments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PostComment postComment = this.Data.PostComments.Find(id);

            this.Data.PostComments.Remove(postComment);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index");
        }
    }
}