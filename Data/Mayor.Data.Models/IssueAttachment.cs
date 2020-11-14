namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IssueAttachment
    {
        public int IssueId { get; set; }

        public Issue Issue { get; set; }

        public string AttachmentId { get; set; }

        public Attachment Attachment { get; set; }
    }
}
