using System.Net;

namespace Infrastructure.Common.Response;

public class Response<T>
{
	private bool Error { get; set; }
	private string Message { get; set; }
	private T Data { get; set; }
	public HttpStatusCode Status { get; set; }

	public Response(T user)
	{
		Error = false;
		Data = user;
	}

	public Response(string message)
	{
		Error = true;
		Message = message;
	}
}