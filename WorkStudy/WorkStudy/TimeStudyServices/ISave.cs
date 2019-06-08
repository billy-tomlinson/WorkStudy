using System.IO;
using System.Threading.Tasks;

namespace TimeStudy.Services
{
    public interface ISave
    {
        //Method to save document as a file and view the saved document
        Task<string> SaveSpreadSheet(string filename, string contentType, MemoryStream stream);
    }
}

