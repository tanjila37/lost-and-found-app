using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Lost_and_Found.Models
{
    public partial class Lostitem
    {

        public int ItemId { get; set; }
        [Required(ErrorMessage = "Item Name is required.")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Item Category is required.")]
        public string ItemCategory { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lost Area is required.")]
        public string LostArea { get; set; }
        [Required(ErrorMessage = "Date is required.")]
        public DateTime? Date { get; set; }
        public string Image { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        [Required(ErrorMessage = "Image is required.")]
        public IFormFile ImageFile { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
