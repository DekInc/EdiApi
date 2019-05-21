using Microsoft.AspNetCore.Http;
using System;

namespace EdiViewer.Models {
    public class UploadFileViewModel {
        public IFormFile File { get; set; }
        public Guid UniqueId { get; set; }
    }
}
