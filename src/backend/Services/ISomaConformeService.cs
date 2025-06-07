using SomaConformeApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SomaConformeApi.Services
{
    public interface ISomaConformeService
    {
        Task<List<SomaConformeCategoriaDto>> SomaConformePorCategoriaAsync();
    }
}
