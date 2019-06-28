using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFF
{
    public interface IComponentService
    {
        Task<List<string>> GetValues();
    }
}