using log4net;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;

namespace SubModuleDynamic365Connection
{
    public class Dynamic365Connection
    {
        private static IOrganizationService _service = null;
        private static readonly ILog log = LogHelper.GetLogger();



        private Dynamic365Connection()
        {
        }
        public static IOrganizationService getInstance()
        {
            try
            {
                if (_service == null)
                {

                    _service = CreateConnect();
                    log.Info("New Instance");

                }
                log.Info("Instance allready esist");
                return _service;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // -- create a connection with dynamic CRM 365
        public static IOrganizationService CreateConnect()
        {
            CrmServiceClient crmSvc = new CrmServiceClient(ConfigurationManager.ConnectionStrings["OAuthConnection"].ConnectionString);
            _service = (IOrganizationService)crmSvc.OrganizationWebProxyClient != null ? (IOrganizationService)crmSvc.OrganizationWebProxyClient : (IOrganizationService)crmSvc.OrganizationServiceProxy;
            Console.WriteLine("success Connection");
            return _service;

        }

        public static IOrganizationService getContext()
        {

            return _service;
        }
    }
}
