﻿//  
// Copyright (c) Microsoft.  All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Commands
{
    using Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Models;
    using System;
    using System.Linq;
    using System.Management.Automation;
    using Properties;
    using Management.ApiManagement.Models;

    [Cmdlet(VerbsCommon.Set,
        Constants.ApiManagementApiRevision,
        DefaultParameterSetName = ExpandedParameterSet,
        SupportsShouldProcess = true)]
    [OutputType(typeof(PsApiManagementApi))]
    public class SetAzureApiManagementApiRevision : SetAzureApiManagementApi
    {   
        [Parameter(
            ParameterSetName = ExpandedParameterSet,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "Identifier of existing API Revision. This parameter is required.")]
        [ValidateNotNullOrEmpty]
        public String ApiRevision { get; set; }

        public override void ExecuteApiManagementCmdlet()
        {
            string resourcegroupName;
            string serviceName;
            string apiId;
            string apiRevision;

            if (ParameterSetName.Equals(ByInputObjectParameterSet))
            {
                resourcegroupName = InputObject.ResourceGroupName;
                serviceName = InputObject.ServiceName;
                apiId = InputObject.ApiId;
                apiRevision = InputObject.ApiRevision;
            }
            else
            {
                resourcegroupName = Context.ResourceGroupName;
                serviceName = Context.ServiceName;
                apiId = ApiId;
                apiRevision = ApiRevision;
            }

            string id = apiId.ApiRevisionIdentifier(apiRevision);

            if (ShouldProcess(id, Resources.SetApiRevision))
            {
                Client.ApiSet(
                    resourcegroupName,
                    serviceName,
                    id,
                    Name,
                    Description,
                    ServiceUrl,
                    Path,
                    Protocols.Distinct().ToArray(),
                    AuthorizationServerId,
                    AuthorizationScope,
                    SubscriptionKeyHeaderName,
                    SubscriptionKeyQueryParamName,
                    InputObject);

                if (PassThru.IsPresent)
                {
                    var api = Client.ApiById(resourcegroupName, serviceName, id);
                    WriteObject(api);
                }
            }
        }
    }
}
