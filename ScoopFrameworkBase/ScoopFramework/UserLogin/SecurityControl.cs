namespace ScoopFramework.UserLogin
{
    public class SecurityControl
    {
        public static bool GetSecurityControl(object userStatus)
        {
            if (userStatus == null)
            {
                return false;
            }
            return true;
        }
    }
}
