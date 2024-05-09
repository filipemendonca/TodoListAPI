using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Domain.Entities
{
    public class Notes : BaseEntity
    {        
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
