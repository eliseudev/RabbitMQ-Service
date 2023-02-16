using System;
namespace RabbitMQ.Models
{
	public class PublishInputModel
	{
		public int FromId { get; set; }
		public int ToId { get; set; }
		public string Content { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}

