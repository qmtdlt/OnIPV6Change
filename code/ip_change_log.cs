using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnIPv6ChangeSend
{
    public class ip_change_log
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long id { get; set; }
        public string? new_ip { get; set; }
        public DateTime? time { get; set; }
    }
}
