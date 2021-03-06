﻿using System.Collections.Generic;

namespace Domain.Entities
{
    public class Artist : BaseEntity
    {
        public string Name { get; set; }
        
        public int DescriptionId { get; set; }
        public Description Description { get; set; }

        public List<Painting> Paintings { get; set; }

        public string ImageName { get; set; }
        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }

        public string Quote { get; set; }
    }
}