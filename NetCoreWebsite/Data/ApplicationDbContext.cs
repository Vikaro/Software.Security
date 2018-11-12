﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreWebsite.Data.Models;

namespace NetCoreWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }

        //public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMessage>().HasKey(t => new { t.UserId, t.MessageId });
            modelBuilder.Entity<Message>()
                .HasOne(p => p.Owner)
                .WithMany(b => b.OwnedMessages);
            modelBuilder.Entity<UserMessage>()
                .HasOne(m => m.Message)
                .WithMany(a => a.Allowed)
                .HasForeignKey(m => m.MessageId);

            modelBuilder.Entity<UserMessage>()
                 .HasOne(m => m.User)
                .WithMany(a => a.AllowedMessages)
                .HasForeignKey(m => m.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}