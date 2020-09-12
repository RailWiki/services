using System;

namespace RailWiki.Shared.Models.Photos
{
    public class ImportPhotoModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PhotoDate { get; set; }
        public ImportGenericLookupModel Location { get; set; }
        public ImportGenericLookupModel CollectionOf { get; set; }
        public ImportLocomotiveModel[] Locomotives { get; set; }
        public string Author { get; set; }
        public string ImageFileUrl { get; set; }
        public string[] Categories { get; set; }
        public ImportGenericLookupModel Album { get; set; }
    }

    public class ImportGenericLookupModel
    {
        public string RefId { get; set; }
        public string Name { get; set; }
    }

    public class ImportLocomotiveModel
    {
        public string RefId { get; set; }
        public string ReportingMarks { get; set; }
        public string Model { get; set; }
    }
}
