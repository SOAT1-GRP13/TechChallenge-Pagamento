public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _fakeResponse;

    public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
    {
        _fakeResponse = responseMessage;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_fakeResponse);
    }
}
