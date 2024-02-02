using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetti.Core.DataTransferObject
{
    public class DocumentsIndexDto
    {
        public string index {  get; set; }
        public List<Message> documents {  get; set; }
    }
}
