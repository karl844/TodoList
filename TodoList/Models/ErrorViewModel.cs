using System;

namespace TodoList.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            Title = "Error";
            Description = "An error occurred while processing your request.";
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}