using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Models.TodoViewModels
{
    public class TodoItemViewModel
    {
        public int ToDoItemId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        
        [Display(Name ="Updated")]
        public DateTime UpdatedDateTime { get; set; }
        public bool IsComplete { get; set; }
        public bool IsChecked { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
