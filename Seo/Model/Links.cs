using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seo.Model
{
   public class Links
    {
        public int Id { get; set; }
        public string SourceTitle { get; set; }
        public string AnchorURL { get; set; }
        public string AnchorText { get; set; }
        public string SourceURL { get; set; }
        public string URLStatus { get; set; }
        public string FinalURL { get; set; }
        public string Catogery { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Guidstr { get; set; } 
    }
}
