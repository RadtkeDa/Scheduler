using Audit.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Scheduler.Domain.Entities
{
    [AuditInclude]
    public class ConfigurationItem
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}