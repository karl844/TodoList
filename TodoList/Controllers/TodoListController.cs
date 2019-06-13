using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;
using TodoList.Models.TodoViewModels;

namespace TodoList.Controllers
{
    [Authorize]
    public class TodoListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [TempData]
        public string Message { get; set; }

        #region CONSTRUCTOR
        public TodoListController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        } 
        #endregion

        #region INDEX
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            List<TodoItemViewModel> toDoItems = await _context.TodoItem
                .Include(t => t.ApplicationUser)
                .Where(u => u.ApplicationUser == user)
                .Select(a => new TodoItemViewModel
                {
                    ToDoItemId = a.ToDoItemId,
                    Title = a.Title,
                    Description = a.Description,
                    UpdatedDateTime = a.UpdatedDateTime,
                    IsComplete = a.IsComplete
                })
                .OrderByDescending(a => a.ToDoItemId)
                .ToListAsync();

            ViewBag.Message = Message;

            return View(toDoItems);
        }
        #endregion

        #region DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .Include(t => t.ApplicationUser)
                .SingleOrDefaultAsync(m => m.ToDoItemId == id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }
        #endregion

        #region CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                DateTime dtNow = DateTime.UtcNow;

                _context.Add(new TodoItem
                {
                    ApplicationUser = user,
                    Title = todoItem.Title,
                    Description = todoItem.Description,
                    CreatedDateTime = dtNow,
                    UpdatedDateTime = dtNow                    
                });

                try
                {
                    await _context.SaveChangesAsync();
                    Message = "Success, a new item has been created";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //log
                    Message = "Error, something went wrong.";                    
                }
            }

            return View(todoItem);
        }
        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);

            TodoItem todoItem = await _context.TodoItem
                .Include(a => a.ApplicationUser)
                .SingleOrDefaultAsync(m => m.ToDoItemId == id && m.ApplicationUser == user);  
            
            if (todoItem == null)
            {
                return NotFound();
            }

            TodoItemViewModel todoItemViewModel = new TodoItemViewModel
            {
                ToDoItemId = todoItem.ToDoItemId,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsComplete = todoItem.IsComplete
            };

            return View(todoItemViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TodoItemViewModel todoItemViewModel)
        {
            if (id != todoItemViewModel.ToDoItemId)
            {
                return NotFound();
            }           

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                TodoItem todoItem = await _context.TodoItem
                    .Include(a => a.ApplicationUser)
                    .SingleOrDefaultAsync(m => m.ToDoItemId == id && m.ApplicationUser == user);

                if (todoItem == null)
                {
                    return NotFound();
                }

                todoItem.Title = todoItemViewModel.Title;
                todoItem.Description = todoItemViewModel.Description;
                todoItem.IsComplete = todoItemViewModel.IsComplete;
                todoItem.UpdatedDateTime = DateTime.UtcNow;

                try
                {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                    Message = "Success, a new item has been updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    //log
                    if (!TodoItemExists(todoItem.ToDoItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Message = "Error, something went wrong.";                        
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(todoItemViewModel);
        }
        #endregion

        #region DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);

            var todoItem = await _context.TodoItem
                .Include(t => t.ApplicationUser)
                .SingleOrDefaultAsync(m => m.ToDoItemId == id && m.ApplicationUser == user);

            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: TodoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            var todoItem = await _context.TodoItem
                .SingleOrDefaultAsync(m => m.ToDoItemId == id && m.ApplicationUser == user);

            _context.TodoItem.Remove(todoItem);
            try
            {
                await _context.SaveChangesAsync();
                Message = "Success, the todo item has been deleted.";
                
            }
            catch (Exception)
            {
                //log
                Message = "There was an error in deleting this to do item.";                
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkComplete(List<TodoItemViewModel> todoItemViewModels)
        {
            if (todoItemViewModels != null)
            {                
                List<TodoItem> todos = await _context.TodoItem.Where(a => todoItemViewModels.Where(b => b.IsChecked).Select(c => c.ToDoItemId).Contains(a.ToDoItemId)).ToListAsync();
                todos.ForEach(a => a.IsComplete = true);
                await _context.SaveChangesAsync();
                Message = "Success, all checked task are now marked complete.";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        #region PRIVATE METHODS
        private bool TodoItemExists(int id)
        {
            return _context.TodoItem.Any(e => e.ToDoItemId == id);
        } 
        #endregion
    }
}
