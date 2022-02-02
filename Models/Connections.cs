using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace task.app.Models
{
    public class Connections
    {
        [Key]
        public Guid Id { get; set; }
        public string SignalrId { get; set; }
        public DateTime TimeStamp { get; set; }
        [JsonIgnore]
        public virtual User UserNavigation { get; set; }
        [ForeignKey("UserNavigation")]
        public int UserId { get; set; }

    }
}