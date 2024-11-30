namespace Presentation.ViewModels
{
    public class CreatePresentationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PresentationName { get; set; }
        public string PresentationDetails { get; set; }
        public IFormFile Photo { get; set; }
    }
}
