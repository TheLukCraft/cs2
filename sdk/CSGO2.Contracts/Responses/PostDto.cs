﻿namespace CS2.Contracts.Responses
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}