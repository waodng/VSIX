using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Logic.NestIn
{
    public interface IRootSelector
    {
        FileItem Select(IEnumerable<FileItem> candidates);
    }
}
