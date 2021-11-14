using Microsoft.Azure.WebJobs.Host;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xrm;

namespace UOP.AzureFunctions.StudentRetention.Helpers.Connections
{
    class UOPeopleCRMConnection
    {
        private readonly UOPOrganizationServiceProxy _orgServiceProxy = null;
        private IOrganizationService _service;
        private XrmServiceContext _xrmServcieContext;

        public UOPeopleCRMConnection(TraceWriter log)
        {
            try
            {
                //Connecting to Dynamics 365 using Xrm.Sdk.Tooling.Connector
                var connectionString = Environment.GetEnvironmentVariable("CrmConnectionString");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var connection = new CrmServiceClient(connectionString);

                if (connection.IsReady == false)
                {
                    log.Error("Connection to CRM is not ready...");
                    throw new Exception("Connection to CRM is not ready...");
                }

                _orgServiceProxy = new UOPOrganizationServiceProxy(connection);
                log.Info("Connected to CRM successfully...");

            }
            catch (Exception ex)
            {
                log.Error($"Error in connection to CRM, Error details: {ex.Message}");
                throw new Exception($"Error in connection to CRM, Error details: {ex.Message}");
            }
        }

        public UOPOrganizationServiceProxy GetService()
        {
            return _orgServiceProxy;
        }

        public UOPOrganizationServiceProxy GetContext()
        {
            return _orgServiceProxy;
        }

        public XrmServiceContext getserviceProvider()
        {
            //returns Dynamics 365 XrmServiceContext
            _service = (IOrganizationService)_orgServiceProxy;
            _xrmServcieContext = new XrmServiceContext(_service);
            return _xrmServcieContext;
        }
    }
}
