namespace LingoMQ.Presenters.WebApi.Requests
{
    public class UploadImageRequest
    {
        public required IFormFile File { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
