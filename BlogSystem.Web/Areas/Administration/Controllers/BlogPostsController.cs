﻿namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Common;
    using Data.Models;
    using Data.UnitOfWork;
    using ViewModels.BlogPost;
    using InputModels.BlogPost;
    using Infrastructure.Mapping;
    using Infrastructure.Identity;

    public class BlogPostsController : AdministrationController
    {
        private readonly IBlogSystemData data;
        private readonly ICurrentUser currentUser;

        public BlogPostsController(IBlogSystemData data, ICurrentUser currentUser)
        {
            this.currentUser = currentUser;
            this.data = data;
        }

        // GET: Administration/BlogPosts
        public ActionResult Index(int page = 1, int perPage = GlobalConstants.DefaultPageSize)
        {
            int pagesCount = (int) Math.Ceiling(this.data.Posts.All().Count() / (decimal) perPage);

            var posts = this.data.Posts
                .All()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedOn)
                .To<BlogPostViewModel>()
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToList();

            var model = new IndexPostsPageViewModel
            {
                Posts = posts,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(model);
        }

        // GET: Administration/BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = this.data.Posts.Find(id);

            if (post == null)
            {
                return this.HttpNotFound();
            }

            return this.View(post);
        }

        // GET: Administration/BlogPosts/Create
        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPostCreateInputModel postInputModel)
        {
            if (postInputModel != null)
            {
                if (this.ModelState.IsValid)
                {
                    var post = new Post
                    {
                        Title = postInputModel.Title,
                        Content = postInputModel.Content,
                        Author = this.currentUser.Get(),
                        AuthorId = this.currentUser.Get().Id,
                    };

                    this.data.Posts.Add(post);
                    this.data.SaveChanges();

                    return this.RedirectToAction("Index");
                }
            }

            return this.View(postInputModel);
        }

        // GET: Administration/BlogPosts/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = this.data.Posts.Find(id);

            if (post == null)
            {
                return this.HttpNotFound();
            }

            var model = new BlogPostEditInputModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
                AuthorId = post.AuthorId
            };

            return this.View(model);
        }

        // POST: Administration/BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogPostEditInputModel postInputModel)
        {
            if (this.ModelState.IsValid)
            {
                var post = this.data.Posts.Find(postInputModel.Id);

                post.Id = postInputModel.Id;
                post.Title = postInputModel.Title;
                post.Content = postInputModel.Content;
                post.AuthorId = postInputModel.AuthorId;
                post.CreatedOn = postInputModel.CreatedOn;

                this.data.Posts.Update(post);
                this.data.SaveChanges();

                return this.RedirectToAction("Index");
            }

            return this.View(postInputModel);
        }

        // GET: Administration/BlogPosts/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = this.data.Posts.Find(id);

            if (post == null)
            {
                return this.HttpNotFound();
            }

            return this.View(post);
        }

        // POST: Administration/BlogPosts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = this.data.Posts.Find(id);

            this.data.Posts.Remove(post);
            this.data.SaveChanges();

            return this.RedirectToAction("Index");
        }
    }
}