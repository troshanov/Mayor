namespace Mayor.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class IssueRequestAttachment
    {
        public int IssueRequestId { get; set; }

        public IssueRequest IssueRequest { get; set; }

        public string AttachmentId { get; set; }

        public Attachment Attachment { get; set; }
    }
}
