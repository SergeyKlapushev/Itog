using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itog
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public virtual List<Message>? MessagesTo { get; set; }
        public virtual List<Message>? MessagesFrom { get; set; }
    }
}
