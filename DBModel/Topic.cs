using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JXW.Crawl.DBModel
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(400)]
        public string title { get; set; }

        [StringLength(16777215)]
        public string content { get; set; }

        [StringLength(20)]
        public string classify { get; set; }

        [StringLength(20)]
        public string publisher { get; set; }

        [StringLength(100)]
        public string source { get; set; }

        [Column(TypeName = "date")]
        public DateTime? publicDate { get; set; }

        public Topic()
        {
            attachments = new HashSet<Attachment>();
        }

        public virtual ICollection<Attachment> attachments { get; set; }
    }
}