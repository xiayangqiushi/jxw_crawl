using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JXW.Crawl.DBModel
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(200)]
        public string fileName { get; set; }

        [StringLength(200)]
        public string filePath { get; set; }

        [StringLength(200)]
        public string sourceUrl { get; set; }

        public int? topicId { get; set; }

        public int downLoadFaild { get; set; }
    }
}