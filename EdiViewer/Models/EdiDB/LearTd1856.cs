using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearTd1856
    {
        public string PackagingCode { get; set; }
        public string LadingQuantity { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
