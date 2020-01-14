using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Natcore.EntityFramework.Logging
{
	public class EventLog
	{
		public int ID { get; set; }

		public DateTimeOffset Timestamp { get; set; }

		public string Level { get; set; }

		public string Message { get; set; }

		public int? EventID { get; set; }

		public string Exception { get; set; }

		public string Host { get; set; }

		public string Username { get; set; }

		public string Browser { get; set; }

		public string Url { get; set; }
	}

	public class EventLogConfiguration : IEntityTypeConfiguration<EventLog>
	{
		public void Configure(EntityTypeBuilder<EventLog> builder)
		{
			builder.HasKey(x => x.ID);
			builder.Property(x => x.Timestamp)
				.IsRequired(true)
				.ValueGeneratedOnAdd()
				.HasDefaultValueSql("SYSUTCDATETIME()");

			builder.Property(x => x.Level).HasMaxLength(20).IsUnicode(false).IsRequired(true);
			builder.Property(x => x.Message).IsRequired(true).IsUnicode(true);
			builder.Property(x => x.EventID).IsRequired(false);
			builder.Property(x => x.Exception).IsUnicode(true);
			builder.Property(x => x.Host).HasMaxLength(30);
			builder.Property(x => x.Username).HasMaxLength(256);
			builder.Property(x => x.Browser).HasMaxLength(200);
			builder.Property(x => x.Url).HasMaxLength(256);
		}
	}
}
