using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Models;

namespace TodoList.Data
{
    public static class SeedDB
    {
        public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Users.Any())
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    Email = "test@email.com",
                    UserName = "test",
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(newUser, "pwd123").Result;

                if (result.Succeeded)
                {
                    context.TodoItem.AddRange(new List<TodoItem>
                    {
                        new TodoItem
                        {
                            ApplicationUser = newUser,
                            CreatedDateTime = DateTime.UtcNow,
                            Title = "coffee",
                            IsComplete = false,
                            Description = "get some more coffee",
                            UpdatedDateTime = DateTime.UtcNow
                        },
                        new TodoItem
                        {
                            ApplicationUser = newUser,
                            CreatedDateTime = DateTime.UtcNow,
                            Title = "complete to do project",
                            IsComplete = false,
                            Description = "complete to do project & submit test to Github",
                            UpdatedDateTime = DateTime.UtcNow
                        },
                        new TodoItem
                        {
                            ApplicationUser = newUser,
                            CreatedDateTime = DateTime.UtcNow,
                            Title = "mark complete",
                            IsComplete = true,
                            Description = "mark this task complete",
                            UpdatedDateTime = DateTime.UtcNow
                        }
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
