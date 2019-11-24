using System.Collections.Generic;

namespace ApiAuth.API.ViewModels
{
    public class ActionEmailTemplateViewModel
    {
        public string Title { get; set; }
        public List<string> Body { get; set; }
        public UrlEmailTemplateViewModel Url { get; set; }
        public SignatureEmailTemplateViewModel Signature { get; set; }
    }

    public class SignatureEmailTemplateViewModel
    {
        public SignatureEmailTemplateViewModel(string managerName, string companyName)
        {
            ManagerName = managerName;
            CompanyName = companyName;
        }

        public string ManagerName { get; private set; }
        public string CompanyName { get; private set; }
    }

    public class UrlEmailTemplateViewModel
    {
        public UrlEmailTemplateViewModel(string title, string link)
        {
            Title = title;
            Link = link;
        }

        public string Link { get; private set; }
        public string Title { get; private set; }
    }
}
