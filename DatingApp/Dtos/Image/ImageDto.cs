﻿namespace DatingApp.Dtos.Image
{
    public sealed class ImageDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string ImageData { get; set; } = null!;
    }
}
