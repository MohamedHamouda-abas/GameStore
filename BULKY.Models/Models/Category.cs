using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BULKY.Models 
{ 
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(256)]
        public string Name { get; set; }=string.Empty;
        [DisplayName("DisPlay Order")]
        [Range(0, 256)]
        public int DisPlayOrder { get; set; }
    }
}
