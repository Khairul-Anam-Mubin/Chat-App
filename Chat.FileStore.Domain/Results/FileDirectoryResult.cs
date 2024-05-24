using Peacious.Framework.Results;

namespace Chat.FileStore.Domain.Results;

public static class FileDirectoryResult
{
   public static IResult<string> FileUpload(this IResult<string> result, string fileId)
   {
        result.SetMessage("File Uploaded SuccessFully.");

        return result;
   }
}
