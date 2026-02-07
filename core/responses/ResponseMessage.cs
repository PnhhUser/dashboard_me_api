public static class ResponsesMessage
{

    public static readonly Func<string, string> CreatedSuccessfully = (string p) => p + " created successfully";
    public static readonly Func<string, string> EditedSuccessfully = (string p) => p + " edited successfully";
    public static readonly Func<string, string> RemovedSuccessfully = (string p) => p + " removed successfully";

}