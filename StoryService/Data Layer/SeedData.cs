﻿using Microsoft.EntityFrameworkCore;
using StoryService.Data_Layer.Models;

namespace StoryService.Data_Layer
{
    public static class SeedData
    {
        public static void seedData(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<StoryServiceDBContext>();

                _context.Database.Migrate();

                Console.WriteLine("Seeding data");

                // List of predefined User IDs
                var userIds = new List<Guid>
                {
                    Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d1"),
                    Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d2"),
                    Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d3"),
                    Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d4"),
                    Guid.Parse("e0be4a36-67cd-4dd6-be48-8b800c3123d5")
                };

                // Seed 20 Story records
                var stories = new List<Story>();
                for (int i = 0; i < 20; i++)
                {
                    var story = new Story
                    {
                        StoryId = Guid.NewGuid(),
                        UserId = userIds[i % userIds.Count],
                        Sound = $"https://example.com/sounds/story_{i + 1}.mp3",
                        CreatedDate = DateTime.UtcNow.AddMinutes(-i * 10),
                    };
                    if (i % 3 == 0) story.Image = "https://images.pexels.com/photos/799443/pexels-photo-799443.jpeg?auto=compress&cs=tinysrgb&w=600";
                    else if (i % 3 == 1) story.Image = "https://images.pexels.com/photos/2486168/pexels-photo-2486168.jpeg?auto=compress&cs=tinysrgb&w=600";
                    else story.Image = "https://images.pexels.com/photos/1226302/pexels-photo-1226302.jpeg?auto=compress&cs=tinysrgb&w=600";
                    stories.Add(story);
                }
                _context.Stories.AddRange(stories);

                //// Seed 20 UserAlreadySeenStory records
                //var userAlreadySeenStories = new List<UserAlreadySeenStory>();
                //foreach (var story in stories)
                //{
                //    // Select random users who have seen this story
                //    var seenUserIds = userIds.OrderBy(x => Guid.NewGuid()).Take(3).ToList();
                //    foreach (var userId in seenUserIds)
                //    {
                //        var userSeenStory = new UserAlreadySeenStory
                //        {
                //            UserId = userId,
                //            StoryId = story.StoryId,
                //            Story = story
                //        };
                //        userAlreadySeenStories.Add(userSeenStory);
                //    }
                //}
                //_context.UserAlreadySeenStories.AddRange(userAlreadySeenStories);

                _context.SaveChanges();
                Console.WriteLine("Data seeding completed");
            }
        }
    }
}
