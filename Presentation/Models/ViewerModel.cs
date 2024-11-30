
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class ViewerModel
    { 
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public PresentationModel Id { get; set; }
        public bool isViewer { get; set; }
        public bool isEditor { get; set; }
    }
}