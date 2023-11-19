using System.Security.Claims;

namespace JwtAspNet.Extentions;

public static class ClaimTypeExtension
{
    public static int Id(this ClaimsPrincipal user)
    {
		try
		{
			return Convert.ToInt32(user.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0");
		}
		catch
		{
			return 0;
		}
    }

	public static string Name (this ClaimsPrincipal user) 
	{
		try
		{
			return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString();
        }
		catch
		{
			return string.Empty;
		}
	}

    public static string GivenName(this ClaimsPrincipal user)
    {
        try
        {
            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

	public static string Image(this ClaimsPrincipal user) 
	{
        try
        {
            return user.Claims.FirstOrDefault(x => x.Type == "Image").Value.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }
}
