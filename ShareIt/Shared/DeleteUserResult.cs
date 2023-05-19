using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareIt.Shared
{
    public class DeleteUserResult
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
