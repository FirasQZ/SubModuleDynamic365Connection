using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.AzureFunctions.StudentRetention.Helpers
{
    class UOPOrganizationServiceProxy : OrganizationServiceContext, IOrganizationService
    {
        private bool ProxyTypesBehaviorIsAdded = false;
        CrmServiceClient connection;
        IOrganizationService IOService;

        public UOPOrganizationServiceProxy(CrmServiceClient connection)
            : base(connection)
        {
            this.connection = connection;
            this.IOService = connection;
        }

        public Guid Create(Entity entity)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            return IOService.Create(entity);

        }
        public new OrganizationResponse Execute(OrganizationRequest request)
        {
            if (ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = false;
            }
            return IOService.Execute(request);
        }
        public void Update(Entity entity)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            IOService.Update(entity);

        }
        public void Delete(string entityName, Guid id)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            IOService.Delete(entityName, id);

        }
        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            IOService.Disassociate(entityName, entityId, relationship, relatedEntities);

        }
        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            IOService.Associate(entityName, entityId, relationship, relatedEntities);
        }
        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            return IOService.Retrieve(entityName, id, columnSet);
        }
        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            if (!ProxyTypesBehaviorIsAdded)
            {
                ProxyTypesBehaviorIsAdded = true;
            }

            return IOService.RetrieveMultiple(query);
        }
    }
}
