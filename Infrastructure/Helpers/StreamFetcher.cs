namespace Infrastructure.Helpers;
public static class StreamFetcher
{
    public static async Task<Stream> FetchStreamAsync(string source, CancellationToken cancellationToken)
    {
        try
        {
            Stream stream;

            if (Uri.IsWellFormedUriString(source, UriKind.Absolute))
            {
                using var httpClient = new HttpClient();
                stream = await httpClient.GetStreamAsync(source, cancellationToken);
            }
            else
            {
                stream = File.OpenRead(source.Replace("\"", ""));
            }

            return stream;
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to retrieve image: " + e);
            throw;
        }
    }
}
