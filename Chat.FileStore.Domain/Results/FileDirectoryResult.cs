using Chat.Framework.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.FileStore.Domain.Results;

public static class FileDirectoryResult
{
   public static IResult<string> FileUpload(this IResult<string> result, string fileId)
   {
        result.SetMessage("File Uploaded SuccessFully.");

        return result;
   }
}
