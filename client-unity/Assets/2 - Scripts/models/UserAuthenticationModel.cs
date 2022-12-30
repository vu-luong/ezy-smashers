public class UserAuthenticationModel
{
    private string username = "";
    private string password = "";

    public string Username { get => username; set => username = value; }
    public string Password { get => password; set => password = value; }

    public UserAuthenticationModel(string username, string password)
    {
        SetUserAuthentication(username, password);
    }

    public void SetUserAuthentication(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
