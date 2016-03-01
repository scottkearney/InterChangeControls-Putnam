using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace Akumina.WebParts.DiscussionBoard
{
    class UserProfileInfo
    {
        public string getUserPic(SPFieldUserValue userValue)
        {
            if (!userValue.ToString().Contains("System Account"))
            {
                string Author = userValue.LookupValue;
                return getUserProfileInfo(userValue.User.ToString());
            }
            return string.Empty;
        }

        public string getUserProfileInfo(string accName)
        {
            UserProfileManager profileManager = new UserProfileManager();
            try
            {
                UserProfile profile = profileManager.GetUserProfile(accName);
                if (profile != null)
                {
                    //Set the Profile
                    string account_name = profile.AccountName;
                    string profilePicture = profile[PropertyConstants.PictureUrl].Value != null ? profile[PropertyConstants.PictureUrl].Value.ToString() : string.Empty;
                    return profilePicture;
                }
            }
            catch (UserNotFoundException)
            {
                // ignore
            }
            return "";
        }
    }
}
